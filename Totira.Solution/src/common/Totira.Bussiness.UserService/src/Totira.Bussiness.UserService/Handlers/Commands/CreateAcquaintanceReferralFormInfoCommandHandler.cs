using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateAcquaintanceReferralFormInfoCommandHandler : IMessageHandler<CreateAcquaintanceReferralFormInfoCommand>
    {

        private readonly ILogger<CreateAcquaintanceReferralFormInfoCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferralFormInfo, Guid> _tenantAcquaintanceReferralFormRepository;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;


        public CreateAcquaintanceReferralFormInfoCommandHandler(
            ILogger<CreateAcquaintanceReferralFormInfoCommandHandler> logger,
            IRepository<TenantAcquaintanceReferralFormInfo, Guid> tenantAcquaintanceReferralFormRepository,
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            IContextFactory contextFactory,
            IMessageService messageService)
        {
            _logger = logger;
            _tenantAcquaintanceReferralFormRepository = tenantAcquaintanceReferralFormRepository;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;

        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateAcquaintanceReferralFormInfoCommand> command)
        {
            await command.MatchAsync(async cmd => {
                Guid? messageOutboxId = null;
                _logger.LogDebug("creating tenant acquaintance referral form info tenant {TenantId} by Referral {ReferralId}", cmd.TenantId, cmd.ReferralId);

                var formResponse = new TenantAcquaintanceReferralFormInfo
                {
                    TenantId = cmd.TenantId,
                    ReferralId = cmd.ReferralId,
                    Comment = cmd.Feedback,
                    Score = cmd.StarScore
                };

                _logger.LogDebug("validate existence of Referal before add form info", cmd.TenantId, cmd.ReferralId);

                var tenantReferrals = await _tenantAcquaintanceReferralsRepository.GetByIdAsync(cmd.TenantId);
                if (tenantReferrals is not null)
                {
                    await _tenantAcquaintanceReferralFormRepository.Add(formResponse);

                    var referral = tenantReferrals.Referrals.Where(r => r.Id == cmd.ReferralId).First();

                    tenantReferrals.Referrals.Remove(referral);

                    referral.Status = "Complete";

                    tenantReferrals.Referrals.Add(referral); 
                    
                    await _tenantAcquaintanceReferralsRepository.Update(tenantReferrals);

                    var tenantAcquaintanceReferralFormUpdatedEvent = new TenantAcquaintanceReferralUpdateEvent(tenantReferrals.Id);
                    var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                    messageOutboxId = await _messageService.SendAsync(notificationContext, tenantAcquaintanceReferralFormUpdatedEvent);
                }
                else
                {
                    _logger.LogError("TenantReferral with id {TenantId} doesn't exist", cmd.TenantId);
                    throw new Exception("Error missing tenantAcquaintanceReferralId in repository"); 
                }
            }, ex => throw ex);

        }
    }
}

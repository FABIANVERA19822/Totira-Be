namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using System.Linq.Expressions;
    using LanguageExt;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Events;
    using Totira.Support.Application.Messages;
    using Totira.Support.Otp;
    using Totira.Support.TransactionalOutbox;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;

    public class UpdateApplicationRequestInvitationCommandHandler : IMessageHandler<UpdateApplicationRequestInvitationCommand>
    {
        private readonly ILogger<UpdateApplicationRequestInvitationCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _tenantApplicationRequestCoapplicantsSendEmailsRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public UpdateApplicationRequestInvitationCommandHandler(
            ILogger<UpdateApplicationRequestInvitationCommandHandler> logger,
            IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> tenantApplicationRequestCoapplicantsSendEmailsRepository,
            IContextFactory contextFactory,
             IMessageService messageService,
             IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantApplicationRequestCoapplicantsSendEmailsRepository = tenantApplicationRequestCoapplicantsSendEmailsRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateApplicationRequestInvitationCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                Expression<Func<TenantApplicationRequestCoapplicantsSendEmails, bool>> expression = tar => tar.ApplicationRequestId == cmd.ApplicationRequestId;

                var info = (await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Get(expression)).Where(x => x.CoapplicantEmail == cmd.CoapplicantEmail).FirstOrDefault();

                if (info != null)
                {
                    info.dateTimeExpiration = cmd.IsActive ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddDays(-1);
                    info.UpdatedBy = cmd.TenantId;
                    info.UpdatedOn = DateTime.Now;

                    await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Update(info);

                    await _otpHandler.UpdateOtpProcessAsync(cmd.ApplicationRequestId, cmd.CoapplicantEmail, cmd.IsActive, 43200);
                }
                else
                {
                    _logger.LogError("Currently there is no record of sending an email to a co-applicant {CoapplicantEmail} with the Application Request {ApplicationRequestId}", cmd.CoapplicantEmail, cmd.ApplicationRequestId);
                    return;
                }
                var tenantApplicationRequestSendEmailUpdatedEvent = new TenantApplicationRequestSendEmailsUpdatedEvent(cmd.TenantId);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantApplicationRequestSendEmailUpdatedEvent);
            }, async ex =>
            {
                var tenantApplicationRequestSendEmailUpdatedEvent = new TenantApplicationRequestSendEmailsUpdatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantApplicationRequestSendEmailUpdatedEvent);
                throw ex;
            });
        }
    }
}
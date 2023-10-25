
namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System.Linq.Expressions;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Enums;
    using Totira.Bussiness.UserService.Events;
    using Totira.Bussiness.UserService.Extensions;
    using Totira.Support.Application.Messages;
    using Totira.Support.TransactionalOutbox;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;
    using LanguageExt;

    public class UpdateTenantApplicationTypeCommandHandler : IMessageHandler<UpdateTenantApplicationTypeCommand>
    {
        private readonly ILogger<UpdateTenantApplicationTypeCommandHandler> _logger;
        private readonly IRepository<TenantApplicationType, Guid> _tenantApplicationTypeRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _tenantApplicationRequestCoapplicantsSendEmailsRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public UpdateTenantApplicationTypeCommandHandler(
            ILogger<UpdateTenantApplicationTypeCommandHandler> logger,
            IRepository<TenantApplicationType, Guid> tenantApplicationTypeRepository,
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> tenantApplicationRequestCoapplicantsSendEmailsRepository,
            IContextFactory contextFactory,
             IMessageService messageService)
        {
            _logger = logger;
            _tenantApplicationTypeRepository = tenantApplicationTypeRepository;
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantApplicationRequestCoapplicantsSendEmailsRepository = tenantApplicationRequestCoapplicantsSendEmailsRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantApplicationTypeCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("Update tenant application type for tenant {cmd.TenantId}", cmd.TenantId);
                Expression<Func<TenantApplicationRequest, bool>> expressionApplication = r => r.TenantId == cmd.TenantId;
                var applicationRequest = (await _tenantApplicationRequestRepository.Get(expressionApplication)).FirstOrDefault();
            
                Expression<Func<TenantApplicationType, bool>> expression = r => r.TenantId == cmd.TenantId; 
                var actualData = (await _tenantApplicationTypeRepository.Get(expression)).FirstOrDefault();

                if (actualData is null)
                    return;

                if ( applicationRequest is not null && cmd.ApplicationType.Equals(ApplicationTypesEnum.Single.GetEnumDescription()))
                {
                    _logger.LogInformation($"Tenant have an application request, it is necessary to eliminate cosigners and emails invitation");

                    await ExpiredCoapplicantsSendEmails(applicationRequest, cmd);

                    applicationRequest.Coapplicants = null;
                    applicationRequest.Guarantor = null;

                    await _tenantApplicationRequestRepository.Update(applicationRequest);
                } 
                
                actualData.ApplicationType = cmd.ApplicationType;
        
                await _tenantApplicationTypeRepository.Update(actualData);

                var tenantUpdatedEvent = new TenantApplicationTypeUpdatedEvent(actualData.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
            }, async ex => {
                var tenantUpdatedEvent = new TenantApplicationTypeUpdatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
                throw ex;
            });
        }

        private async Task ExpiredCoapplicantsSendEmails(TenantApplicationRequest applicationRequest, UpdateTenantApplicationTypeCommand command)
        {
            Expression<Func<TenantApplicationRequestCoapplicantsSendEmails, bool>> expressionMails = r => r.ApplicationRequestId == applicationRequest.Id;
            var coapplicantsSendEmails = await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Get(expressionMails);

            foreach( var mail  in coapplicantsSendEmails)
            {
                _logger.LogInformation($"Expired email: {mail.CoapplicantEmail} of application request: {applicationRequest.Id} in ExpiredCoapplicantsSendEmails when ApplicationType Changes");
                
                mail.dateTimeExpiration = DateTimeOffset.UtcNow.AddDays(-1);
                mail.UpdatedOn = DateTimeOffset.UtcNow;
                mail.UpdatedBy = command.TenantId;

                await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Update(mail);
            }
        }
    }
}

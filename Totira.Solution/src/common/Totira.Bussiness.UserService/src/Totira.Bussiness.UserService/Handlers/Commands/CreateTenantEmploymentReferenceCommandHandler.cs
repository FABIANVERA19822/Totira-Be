using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantEmploymentReferenceCommandHandler : IMessageHandler<CreateTenantEmploymentReferenceCommand>
    {
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly ILogger<CreateTenantEmploymentReferenceCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantEmploymentReferenceCommandHandler(
           IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
           ILogger<CreateTenantEmploymentReferenceCommandHandler> logger,
           IContextFactory contextFactory,
           IMessageService messageService)
        {
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;    
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantEmploymentReferenceCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant Employment Reference with id {TenantId}", cmd.TenantId);

                var tenantEmploymentReference = new TenantEmploymentReference()
                {
                    Id = cmd.TenantId,
                    FirstName = cmd.FirstName,
                    LastName = cmd.LastName,
                    JobTitle = cmd.JobTitle,
                    Relationship = cmd.Relationship,
                    Email = cmd.Email,
                    PhoneNumber = new Domain.EmploymentReferencePhoneNumber(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                    CreatedOn = DateTimeOffset.Now,
                };

                await _tenantEmploymentReferenceRepository.Add(tenantEmploymentReference);
                var objectEvent = new TenantEmploymentReferenceCreatedEvent(tenantEmploymentReference.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }, async ex => {
                var objectEvent = new TenantEmploymentReferenceCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });
        }
    }
}

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
    public class UpdateTenantEmploymentReferenceCommandHandler : IMessageHandler<UpdateTenantEmploymentReferenceCommand>
    {
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly ILogger<UpdateTenantEmploymentReferenceCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public UpdateTenantEmploymentReferenceCommandHandler(
           IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
           ILogger<UpdateTenantEmploymentReferenceCommandHandler> logger,
           IContextFactory contextFactory,
           IMessageService messageService)
        {
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _logger = logger;
            _messageService = messageService;
            _contextFactory = contextFactory;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantEmploymentReferenceCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("updating tenant Employment Reference with id {TenantId}", cmd.TenantId);

                var actualData = await _tenantEmploymentReferenceRepository.GetByIdAsync(cmd.TenantId);

                actualData.FirstName = actualData.FirstName != cmd.FirstName ? cmd.FirstName : actualData.FirstName;
                actualData.LastName = actualData.LastName != cmd.LastName ? cmd.LastName : actualData.LastName;
                actualData.JobTitle = actualData.JobTitle != cmd.JobTitle ? cmd.JobTitle : actualData.JobTitle;
                actualData.Relationship = actualData.Relationship != cmd.Relationship ? cmd.Relationship : actualData.Relationship;
                actualData.Email = actualData.Email != cmd.Email ? cmd.Email : actualData.Email;
                actualData.PhoneNumber.Number = actualData.PhoneNumber.Number != cmd.PhoneNumber.Number ? cmd.PhoneNumber.Number : actualData.PhoneNumber.Number;
                actualData.PhoneNumber.CountryCode = actualData.PhoneNumber.CountryCode != cmd.PhoneNumber.CountryCode ? cmd.PhoneNumber.CountryCode : actualData.PhoneNumber.CountryCode;

                await _tenantEmploymentReferenceRepository.Update(actualData);

                var tenantUpdatedEvent = new TenantEmploymentReferenceUpdatedEvent(actualData.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
            }, async ex => {
                var tenantUpdatedEvent = new TenantEmploymentReferenceUpdatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
                throw ex;
            });
        }
    }
}


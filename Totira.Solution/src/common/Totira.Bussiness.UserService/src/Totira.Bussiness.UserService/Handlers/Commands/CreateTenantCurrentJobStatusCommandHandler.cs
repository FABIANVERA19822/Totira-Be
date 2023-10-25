
using Microsoft.Extensions.Logging;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantCurrentJobStatusCommandHandler : IMessageHandler<CreateTenantCurrentJobStatusCommand>
    {
        private readonly ILogger<CreateTenantCurrentJobStatusCommandHandler> _logger;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _tenantCurrentJobRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantCurrentJobStatusCommandHandler(
            ILogger<CreateTenantCurrentJobStatusCommandHandler> logger, 
            IRepository<TenantCurrentJobStatus, Guid> tenantCurrentJobRepository,
             IContextFactory contextFactory,
             IMessageService messageService)
        {
            _logger = logger;
            _tenantCurrentJobRepository = tenantCurrentJobRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }


        /// <summary>
        /// Create a new TenantCurrentJobStatus for the tenant 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(IContext context, CreateTenantCurrentJobStatusCommand command)
        {

            _logger.LogInformation("creating Tenant Current Job Status with id {tenantId}", command.TenantId);

            var tenantCurrentJobStatus = new TenantCurrentJobStatus(command.TenantId, command.CurrentJobStatus, 
                command.IsUnderRevisionSend, DateTimeOffset.UtcNow);
            
            await _tenantCurrentJobRepository.Add(tenantCurrentJobStatus);

            var userCreatedEvent = new TenantCurrentJobStatusCreatedEvent(tenantCurrentJobStatus.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
        }
    }
}

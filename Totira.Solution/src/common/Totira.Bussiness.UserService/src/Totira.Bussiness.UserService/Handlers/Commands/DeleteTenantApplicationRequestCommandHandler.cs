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
    public class DeleteTenantApplicationRequestCommandHandler : IMessageHandler<DeleteTenantApplicationRequestCommand>
    {
        private readonly ILogger<DeleteTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public DeleteTenantApplicationRequestCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            ILogger<DeleteTenantApplicationRequestCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, DeleteTenantApplicationRequestCommand command)
        {
            _logger.LogDebug($"deleting application request for tenant id {command.TenantId} requestId {command.ApplicationRequestId}");

            var current = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if (current == null || current.TenantId != command.TenantId)
            {
                _logger.LogError($"The application request {command.ApplicationRequestId} dont exist or the tenant {command.TenantId} is not the owner of them");
                return;
            }

            await _tenantApplicationRequestRepository.DeleteByIdAsync(command.ApplicationRequestId);

            var tenantApplicationRequestDeletedEvent = new TenantApplicationRequestDeletedEvent(command.ApplicationRequestId);

            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantApplicationRequestDeletedEvent);
        }
    }
}
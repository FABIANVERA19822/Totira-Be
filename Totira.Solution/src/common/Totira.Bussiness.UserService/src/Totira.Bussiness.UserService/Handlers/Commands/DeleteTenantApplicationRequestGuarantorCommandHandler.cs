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
    public class DeleteTenantApplicationRequestGuarantorCommandHandler : IMessageHandler<DeleteTenantApplicationRequestGuarantorCommand>
    {       
        private readonly ILogger<DeleteTenantApplicationRequestGuarantorCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public DeleteTenantApplicationRequestGuarantorCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            ILogger<DeleteTenantApplicationRequestGuarantorCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
            _messageService = messageService;
            _contextFactory = contextFactory;

        }
        public async Task HandleAsync(IContext context, DeleteTenantApplicationRequestGuarantorCommand command)
        {
            _logger.LogDebug($"deleting guarantor for application request for tenant id {command.TenantId} requestId {command.ApplicationRequestId} guarantorid {command.GuarantorId}");

            var current = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if (current == null || current.TenantId != command.TenantId || current.Guarantor == null || current.Guarantor.Id != command.GuarantorId)
            {
                _logger.LogError($"The application request {command.ApplicationRequestId} dont exist or the tenant {command.TenantId} is not the owner of them or the guarantor is not valid");
                return;
            }

            current.Guarantor = null;

            await _tenantApplicationRequestRepository.Update(current);

            var objectEvent = new TenantApplicationRequestGuarantorDeletedEvent(command.ApplicationRequestId);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
        }
    }
}

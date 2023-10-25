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
    public class DeleteTenantApplicationRequestCoapplicantCommandHandler : IMessageHandler<DeleteTenantApplicationRequestCoapplicantCommand>
    {
        private readonly ILogger<DeleteTenantApplicationRequestCoapplicantCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public DeleteTenantApplicationRequestCoapplicantCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            ILogger<DeleteTenantApplicationRequestCoapplicantCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, DeleteTenantApplicationRequestCoapplicantCommand command)
        {
            _logger.LogDebug($"deleting guarantor for application request for tenant id {command.TenantId} requestId {command.ApplicationRequestId} coapplicantId {command.CoapplicantId}");

            var current = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if (current == null || current.TenantId != command.TenantId || current.Coapplicants == null || !current.Coapplicants.Any(ca => ca.Id == command.CoapplicantId))
            {
                _logger.LogError($"The application request {command.ApplicationRequestId} dont exist or the tenant {command.TenantId} is not the owner of them or the guarantor is not valid");
                return;
            }

            var toRemove = current.Coapplicants.First(ca => ca.Id == command.CoapplicantId);

            current.Coapplicants.Remove(toRemove);

            await _tenantApplicationRequestRepository.Update(current);

            var tenantApplicationRequestCoapplicantDeletedEvent = new TenantApplicationRequestCoapplicantDeletedEvent(command.ApplicationRequestId);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantApplicationRequestCoapplicantDeletedEvent);
        }
    }
}
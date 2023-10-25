using static Totira.Support.Application.Messages.IMessageHandler;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Events;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class DeleteTenantApplicationRequestCommandHandler : IMessageHandler<DeleteTenantApplicationRequestCommand>
    {
        private readonly ILogger<DeleteTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        public DeleteTenantApplicationRequestCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            ILogger<DeleteTenantApplicationRequestCommandHandler> logger
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;


        }
        public async Task HandleAsync(IContext context, DeleteTenantApplicationRequestCommand command)
        {
            _logger.LogDebug($"deleting application request for tenant id {command.TenantId} requestId {command.ApplicationRequestId}");

            var current = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if ( current == null  || current.TenantId != command.TenantId) {
                _logger.LogError($"The application request {command.ApplicationRequestId} dont exist or the tenant {command.TenantId} is not the owner of them");
                return;            
            }


            await _tenantApplicationRequestRepository.DeleteByIdAsync(command.ApplicationRequestId);

            var userCreatedEvent = new TenantApplicationRequestDeletedEvent(command.ApplicationRequestId);
        }
    }
}

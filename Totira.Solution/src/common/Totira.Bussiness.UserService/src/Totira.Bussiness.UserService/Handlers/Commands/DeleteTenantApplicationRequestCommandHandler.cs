using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using LanguageExt;

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

        public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantApplicationRequestCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("deleting application request for tenant id {TenantId} requestId {ApplicationRequestId}", cmd.TenantId, cmd.ApplicationRequestId);

                var current = await _tenantApplicationRequestRepository.GetByIdAsync(cmd.ApplicationRequestId);

                if ( current == null  || current.TenantId != cmd.TenantId) {
                    _logger.LogError("The application request {ApplicationRequestId} dont exist or the tenant {TenantId} is not the owner of them", cmd.ApplicationRequestId, cmd.TenantId);
                    return;            
                }


                await _tenantApplicationRequestRepository.DeleteByIdAsync(cmd.ApplicationRequestId);

                var objectEvent = new TenantApplicationRequestDeletedEvent(cmd.ApplicationRequestId);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }
            , async ex => {
                var objectEvent = new TenantApplicationRequestDeletedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });
        }
    }
}
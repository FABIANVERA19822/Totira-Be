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
        public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantApplicationRequestGuarantorCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("deleting guarantor for application request for tenant id {TenantId} requestId {ApplicationRequestId} guarantorid {GuarantorId}", cmd.TenantId, cmd.ApplicationRequestId, cmd.GuarantorId);

                var current = await _tenantApplicationRequestRepository.GetByIdAsync(cmd.ApplicationRequestId);

                if (current == null || current.TenantId != cmd.TenantId || current.Guarantor == null || current.Guarantor.Id != cmd.GuarantorId)
                {
                    _logger.LogError("The application request {ApplicationRequestId} dont exist or the tenant {TenantId} is not the owner of them or the guarantor is not valid", cmd.ApplicationRequestId, cmd.TenantId);
                    return;
                }

                current.Guarantor = null;

                await _tenantApplicationRequestRepository.Update(current);

                var objectEvent = new TenantApplicationRequestGuarantorDeletedEvent(cmd.ApplicationRequestId);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }, async ex => {
                var objectEvent = new TenantApplicationRequestGuarantorDeletedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });
        }
    }
}

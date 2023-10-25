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

        public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantApplicationRequestCoapplicantCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("deleting guarantor for application request for tenant id {TenantId} requestId {ApplicationRequestId} coapplicantId {CoapplicantId}", cmd.TenantId, cmd.ApplicationRequestId, cmd.CoapplicantId);

                var current = await _tenantApplicationRequestRepository.GetByIdAsync(cmd.ApplicationRequestId);

                if (current == null || current.TenantId != cmd.TenantId || current.Coapplicants == null || !current.Coapplicants.Any(ca => ca.Id == cmd.CoapplicantId))
                {
                    _logger.LogError($"The application request {cmd.ApplicationRequestId} dont exist or the tenant {cmd.TenantId} is not the owner of them or the guarantor is not valid");
                    return;
                }

                var toRemove = current.Coapplicants.First(ca => ca.Id == cmd.CoapplicantId);

                current.Coapplicants.Remove(toRemove);

                await _tenantApplicationRequestRepository.Update(current);

                var objectEvent = new TenantApplicationRequestCoapplicantDeletedEvent(cmd.ApplicationRequestId);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }, async ex => {
                var objectEvent = new TenantApplicationRequestCoapplicantDeletedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });

        }
    }
}
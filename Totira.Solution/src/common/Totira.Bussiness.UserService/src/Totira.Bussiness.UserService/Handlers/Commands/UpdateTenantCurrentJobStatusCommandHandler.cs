
using Microsoft.Extensions.Logging;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantCurrentJobStatusCommandHandler : IMessageHandler<UpdateTenantCurrentJobStatusCommand>
    {
        private readonly ILogger<UpdateTenantCurrentJobStatusCommandHandler> _logger;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _tenantCurrentJobRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public UpdateTenantCurrentJobStatusCommandHandler(
            ILogger<UpdateTenantCurrentJobStatusCommandHandler> logger, 
            IRepository<TenantCurrentJobStatus, Guid> tenantCurrentJobRepository,
            IContextFactory contextFactory,
             IMessageService messageService)
        {
            _logger = logger;
            _tenantCurrentJobRepository = tenantCurrentJobRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, UpdateTenantCurrentJobStatusCommand command)
        {
            _logger.LogDebug($"Update Tenant Current Job Status type for tenant {command.TenantId}");

            Expression<Func<TenantCurrentJobStatus, bool>> expression = (p => p.TenantId == command.TenantId);

            var actualData = (await _tenantCurrentJobRepository.Get(expression)).FirstOrDefault();

            if (actualData is null)
                return;
            if (command.UpdateCurrentJobStatus)
            {
                actualData.CurrentJobStatus = command.CurrentJobStatus;
            }
            actualData.IsUnderRevisionSend = command.IsUnderRevisionSend;
            actualData.UpdatedOn = DateTime.UtcNow;

            await _tenantCurrentJobRepository.Update(actualData);

            var tenantUpdatedEvent = new TenantCurrentJobStatusUpdatedEvent(actualData.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
        }
    }
}

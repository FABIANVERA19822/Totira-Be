using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class AcceptTermsAndConditionsCommandHandler : IMessageHandler<AcceptTermsAndConditionsCommand>
    {
        private readonly IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> _repository;
        private readonly ILogger<AcceptTermsAndConditionsCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public AcceptTermsAndConditionsCommandHandler(
            IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> repository,
            ILogger<AcceptTermsAndConditionsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {

            _repository = repository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }



        public async Task HandleAsync(IContext context, AcceptTermsAndConditionsCommand command)
        {
            Guid? messageOutboxId = null;
            _logger.LogDebug($"saving tenant Terms And Conditiond Acceptance Information For tenant Id : {command.TenantId}");
            TenantTermsAndConditionsAcceptanceInfo tenantTermsAndConditionsAcceptanceInfo = new TenantTermsAndConditionsAcceptanceInfo()
            {
                TenantId = command.TenantId,
                SigningDateTime = command.SigningDateTime,
            };
            await _repository.Add(tenantTermsAndConditionsAcceptanceInfo);
            var termsAndConditionsAcceptanceEvent = new AcceptTermsAndConditionsCreatedEvent(tenantTermsAndConditionsAcceptanceInfo.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, termsAndConditionsAcceptanceEvent);
        }
    }
}

using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class AcceptTermsAndConditionsCommandHandler : BaseMessageHandler<AcceptTermsAndConditionsCommand, AcceptTermsAndConditionsCreatedEvent>
    {
        private readonly IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> _repository;

        public AcceptTermsAndConditionsCommandHandler(
            IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> repository,
            ILogger<AcceptTermsAndConditionsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService)
            : base(logger, contextFactory, messageService)
        {
            _repository = repository;
        }

        protected override async Task<AcceptTermsAndConditionsCreatedEvent> Process(IContext context, AcceptTermsAndConditionsCommand command)
        {
            _logger.LogDebug("saving tenant Terms And Conditiond Acceptance Information For tenant Id : {TenantId}", command.TenantId);
            var tenantTermsAndConditionsAcceptanceInfo = new TenantTermsAndConditionsAcceptanceInfo()
            {
                TenantId = command.TenantId,
                SigningDateTime = command.SigningDateTime,
            };
            await _repository.Add(tenantTermsAndConditionsAcceptanceInfo);
            return new AcceptTermsAndConditionsCreatedEvent(tenantTermsAndConditionsAcceptanceInfo.Id);
        }
    }
}

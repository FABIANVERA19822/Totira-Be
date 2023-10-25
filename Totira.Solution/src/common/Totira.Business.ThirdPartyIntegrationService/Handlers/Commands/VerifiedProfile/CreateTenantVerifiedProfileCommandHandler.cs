using static Totira.Support.Application.Messages.IMessageHandler;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Support.Application.Messages;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile;

namespace Totira.Bussiness.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile
{
    public class CreateTenantVerifiedProfileCommandHandler : IMessageHandler<CreateTenantVerifiedProfileCommand>
    {
        private readonly ILogger<CreateTenantVerifiedProfileCommandHandler> _logger;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;

        public CreateTenantVerifiedProfileCommandHandler(ILogger<CreateTenantVerifiedProfileCommandHandler> logger,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository)
        {
            _logger = logger;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
        }

        public async Task HandleAsync(IContext context, CreateTenantVerifiedProfileCommand command)
        {
            _logger.LogDebug($"creating tenant Verified Profile for tenant {command.TenantId}");

            var tenantVerifiedProfile = new TenantVerifiedProfile
            {
                Id = Guid.NewGuid(),
                TenantId = command.TenantId,
                Certn = command.Certn,
                Jira = command.Jira,
                Persona = command.Persona,
                IsVerifiedEmailConfirmation = command.IsVerifiedEmailConfirmation,
            };
            await _tenantVerifiedProfileRepository.Add(tenantVerifiedProfile);
        }
    }
}

using static Totira.Support.Application.Messages.IMessageHandler;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Support.Application.Messages;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using LanguageExt;

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

        public async Task HandleAsync(IContext context, Either<Exception,CreateTenantVerifiedProfileCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant Verified Profile for tenant {TenantId}", cmd.TenantId);

                var tenantVerifiedProfile = new TenantVerifiedProfile
                {
                    Id = Guid.NewGuid(),
                    TenantId = cmd.TenantId,
                    Certn = cmd.Certn,
                    Jira = cmd.Jira,
                    Persona = cmd.Persona,
                    IsVerifiedEmailConfirmation = cmd.IsVerifiedEmailConfirmation,
                };
                await _tenantVerifiedProfileRepository.Add(tenantVerifiedProfile);
            }, ex => throw ex);
        }
    }
}


using Microsoft.Extensions.Logging;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using LanguageExt;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile
{
    public class CreateTenantGroupVerifiedProfileCommandHandler : IMessageHandler<CreateTenantGroupVerifiedProfileCommand>
    {
        private readonly ILogger<CreateTenantGroupVerifiedProfileCommandHandler> _logger;
        private readonly IRepository<TenantGroupVerifiedProfile, Guid> _tenantGroupVerifiedProfileRepository;

        public CreateTenantGroupVerifiedProfileCommandHandler(ILogger<CreateTenantGroupVerifiedProfileCommandHandler> logger,
            IRepository<TenantGroupVerifiedProfile, Guid> tenantGroupVerifiedProfileRepository)
        {
            _logger = logger;
            _tenantGroupVerifiedProfileRepository = tenantGroupVerifiedProfileRepository;
        }

        public async Task HandleAsync(IContext context, Either<Exception,CreateTenantGroupVerifiedProfileCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant Group Verified Profile for tenant {TenantId}", cmd.TenantId);

                var tenantGroupVerifiedProfile = new TenantGroupVerifiedProfile
                {
                    Id = Guid.NewGuid(),
                    TenantId = cmd.TenantId,
                    Certn = cmd.Certn,
                    Jira = cmd.Jira,
                    Persona = cmd.Persona,
                    IsVerifiedEmailConfirmation = cmd.IsVerifiedEmailConfirmation,
                };
                await _tenantGroupVerifiedProfileRepository.Add(tenantGroupVerifiedProfile);
            }, ex => throw ex);
        }
    }
}

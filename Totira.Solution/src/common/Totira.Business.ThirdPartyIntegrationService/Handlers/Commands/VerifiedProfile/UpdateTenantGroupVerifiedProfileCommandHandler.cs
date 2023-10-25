
using Microsoft.Extensions.Logging;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using LanguageExt;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile
{
    public class UpdateTenantGroupVerifiedProfileCommandHandler : IMessageHandler<UpdateTenantGroupVerifiedProfileCommand>
    {
        private readonly ILogger<UpdateTenantGroupVerifiedProfileCommandHandler> _logger;
        private readonly IRepository<TenantGroupVerifiedProfile, Guid> _tenantGroupVerifiedProfileRepository;

        public UpdateTenantGroupVerifiedProfileCommandHandler(ILogger<UpdateTenantGroupVerifiedProfileCommandHandler> logger,
            IRepository<TenantGroupVerifiedProfile, Guid> tenantGroupVerifiedProfileRepository)
        {
            _logger = logger;
            _tenantGroupVerifiedProfileRepository = tenantGroupVerifiedProfileRepository;
        }

        public async Task HandleAsync(IContext context, Either<Exception,UpdateTenantGroupVerifiedProfileCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("Update tenant Group Verified Profile for tenant {TenantId}", cmd.TenantId);

                Expression<Func<TenantGroupVerifiedProfile, bool>> predicate = p => p.TenantId == cmd.TenantId;
                var stored = (await _tenantGroupVerifiedProfileRepository.Get(predicate)).FirstOrDefault();

                if (stored == null)
                    return;

                stored.UpdatedOn = DateTime.UtcNow;

                var tenantGroupVerifiedProfile = new TenantGroupVerifiedProfile
                {
                    Id = stored.Id,
                    TenantId = stored.TenantId,
                    Certn = cmd.Certn.HasValue && stored.Certn != cmd.Certn ? cmd.Certn.Value : stored.Certn,
                    Jira = cmd.Jira.HasValue && stored.Jira != cmd.Jira ? cmd.Jira.Value : stored.Jira,
                    Persona = cmd.Persona.HasValue && stored.Persona != cmd.Persona ? cmd.Persona.Value : stored.Persona,
                    IsVerifiedEmailConfirmation = cmd.IsVerifiedEmailConfirmation
                };
                await _tenantGroupVerifiedProfileRepository.Update(tenantGroupVerifiedProfile);
            }, ex => throw ex);
        }
    }
}

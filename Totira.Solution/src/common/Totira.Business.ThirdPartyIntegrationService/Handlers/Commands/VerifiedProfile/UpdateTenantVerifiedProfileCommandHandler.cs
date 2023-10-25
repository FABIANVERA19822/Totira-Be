
using LanguageExt;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile
{
    public class UpdateTenantVerifiedProfileCommandHandler : IMessageHandler<UpdateTenantVerifiedProfileCommand>
    {
        private readonly ILogger<UpdateTenantVerifiedProfileCommandHandler> _logger;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;

        public UpdateTenantVerifiedProfileCommandHandler(ILogger<UpdateTenantVerifiedProfileCommandHandler> logger,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository)
        {
            _logger = logger;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
        }

        public async Task HandleAsync(IContext context, Either<Exception,UpdateTenantVerifiedProfileCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("Update tenant Verified Profile for tenant {TenantId}", cmd.TenantId);

                Expression<Func<TenantVerifiedProfile, bool>> predicate = p => p.TenantId == cmd.TenantId;
                var stored = (await _tenantVerifiedProfileRepository.Get(predicate)).FirstOrDefault();

                if (stored == null)
                    return;

                stored.UpdatedOn = DateTime.UtcNow;

                var tenantVerifiedProfile = new TenantVerifiedProfile
                {
                    Id = stored.Id,
                    TenantId = stored.TenantId,
                    Certn = cmd.Certn.HasValue && stored.Certn != cmd.Certn ? cmd.Certn.Value : stored.Certn,
                    Jira = cmd.Jira.HasValue && stored.Jira != cmd.Jira ? cmd.Jira.Value : stored.Jira,
                    Persona = cmd.Persona.HasValue && stored.Persona != cmd.Persona ? cmd.Persona.Value : stored.Persona,
                    IsVerifiedEmailConfirmation = cmd.IsVerifiedEmailConfirmation
                };
                await _tenantVerifiedProfileRepository.Update(tenantVerifiedProfile);
            }, ex => throw ex);
        }
    }
}

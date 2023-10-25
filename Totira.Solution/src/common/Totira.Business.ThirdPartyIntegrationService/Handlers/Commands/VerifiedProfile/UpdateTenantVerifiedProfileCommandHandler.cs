
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

        public async Task HandleAsync(IContext context, UpdateTenantVerifiedProfileCommand command)
        {
            _logger.LogDebug($"Update tenant Verified Profile for tenant {command.TenantId}");

            Expression<Func<TenantVerifiedProfile, bool>> predicate = p => p.TenantId == command.TenantId;
            var stored = (await _tenantVerifiedProfileRepository.Get(predicate)).FirstOrDefault();

            if (stored == null)
            {
                return;
            }

            stored.UpdatedOn = DateTime.UtcNow;

            var tenantVerifiedProfile = new TenantVerifiedProfile
            {
                Id = stored.Id,
                TenantId = stored.TenantId,
                Certn = command.Certn.HasValue && stored.Certn != command.Certn ? command.Certn.Value : stored.Certn,
                Jira = command.Jira.HasValue && stored.Jira != command.Jira ? command.Jira.Value : stored.Jira,
                Persona = command.Persona.HasValue && stored.Persona != command.Persona ? command.Persona.Value : stored.Persona,
                IsVerifiedEmailConfirmation = command.IsVerifiedEmailConfirmation
            };
            await _tenantVerifiedProfileRepository.Update(tenantVerifiedProfile);
        }
    }
}

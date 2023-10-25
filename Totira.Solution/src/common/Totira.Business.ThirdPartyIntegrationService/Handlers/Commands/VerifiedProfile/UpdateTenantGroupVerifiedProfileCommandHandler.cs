
using Microsoft.Extensions.Logging;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;

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

        public async Task HandleAsync(IContext context, UpdateTenantGroupVerifiedProfileCommand command)
        {
            _logger.LogDebug($"Update tenant Group Verified Profile for tenant {command.TenantId}");

            Expression<Func<TenantGroupVerifiedProfile, bool>> predicate = p => p.TenantId == command.TenantId;
            var stored = (await _tenantGroupVerifiedProfileRepository.Get(predicate)).FirstOrDefault();

            if (stored == null)
            {
                return;
            }

            stored.UpdatedOn = DateTime.UtcNow;

            var tenantGroupVerifiedProfile = new TenantGroupVerifiedProfile
            {
                Id = stored.Id,
                TenantId = stored.TenantId,
                Certn = command.Certn.HasValue && stored.Certn != command.Certn ? command.Certn.Value : stored.Certn,
                Jira = command.Jira.HasValue && stored.Jira != command.Jira ? command.Jira.Value : stored.Jira,
                Persona = command.Persona.HasValue && stored.Persona != command.Persona ? command.Persona.Value : stored.Persona,
                IsVerifiedEmailConfirmation = command.IsVerifiedEmailConfirmation
            };
            await _tenantGroupVerifiedProfileRepository.Update(tenantGroupVerifiedProfile);
        }
    }
}

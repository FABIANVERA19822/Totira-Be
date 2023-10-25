using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Business.ThirdPartyIntegrationService.Commands.Persona;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService;
using Totira.Business.ThirdPartyIntegrationService.Events.Persona;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Persona
{
    public class TenantPersonaValidationCommandHandler : IMessageHandler<TenantPersonaValidationCommand>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly PersonaOptions _personaOptions;
        private readonly IRepository<TenantPersonaValidation, string> _tenantPersonalValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly ILogger<TenantPersonaValidationCommandHandler> _logger;

        public TenantPersonaValidationCommandHandler(
            IRepository<TenantPersonaValidation, string> tenantPersonalValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<RestClientOptions> restClientOptions,
            IOptions<PersonaOptions> personaOptions,
            ILogger<TenantPersonaValidationCommandHandler> logger,
            IQueryRestClient queryRestClient)
        {
            _tenantPersonalValidationRepository = tenantPersonalValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _restClientOptions = restClientOptions.Value;
            _personaOptions = personaOptions.Value;
            _queryRestClient = queryRestClient;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, TenantPersonaValidationCommand command)
        {
            var templateId = command.data.attributes.payload.included.FirstOrDefault(x => x.type == "inquiry-template").id;

            if (templateId != _personaOptions.TemplateId)
            {
                return;
            }

            var tenantId = Guid.Parse(command.data.attributes.payload.data.attributes.referenceid);

            var tenantInfo = await _queryRestClient.GetAsync<GetTenantContactInformationDto>($"{_restClientOptions.User}/user/tenant/contactinfo/{tenantId}");

            if (string.IsNullOrEmpty(tenantInfo.Content.Email))
            {
                _logger.LogError($"Tenant with id {tenantId} dont exist");
                return;
            }

            var currentInquiry = await _tenantPersonalValidationRepository.GetByIdAsync(command.data.attributes.payload.data.id);

            if (currentInquiry == null)
            {
                await CreateInquiryTenant(command, tenantId);
                var inquiryCreatedEvent = new TenantPersonaValidationCreatedEvent(tenantId);

                // Create TenantVerifiedProfile
                var tenantProfile = TenantVerifiedProfile.CreateVerifiedProfile(
                   tenantId,
                    false,
                    false,
                    false,
                    false);
                await _tenantVerifiedProfileRepository.Add(tenantProfile);
            }
            else
            {
                if (currentInquiry.Status != command.data.attributes.payload.data.attributes.status)
                {
                    await UpdateInquityTenant(currentInquiry, command);
                    var inquiryUpdatedEvent = new TenantPersonaValidationUpdatedEvent(tenantId);

                    if (currentInquiry.Status == "completed" || currentInquiry.Status == "rejected")
                    {
                        Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == tenantId);
                        var verifications = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();
                        verifications.Persona = true;
                        verifications.UpdatedOn = DateTime.UtcNow;
                        await _tenantVerifiedProfileRepository.Update(verifications);
                    }
                }
            }
        }

        private async Task UpdateInquityTenant(TenantPersonaValidation currentInquiry, TenantPersonaValidationCommand command)
        {
            currentInquiry.UpdatedOn = DateTime.Now;
            currentInquiry.Status = command.data.attributes.payload.data.attributes.status;
            currentInquiry.UrlImages = command.GetPhotos();

            await _tenantPersonalValidationRepository.Update(currentInquiry);
        }

        private async Task CreateInquiryTenant(TenantPersonaValidationCommand command, Guid tenantId)
        {
            TenantPersonaValidation tenantPersonaValidation = new TenantPersonaValidation();
            tenantPersonaValidation.Id = command.data.attributes.payload.data.id;
            tenantPersonaValidation.TenantId = tenantId;
            tenantPersonaValidation.CreatedOn = DateTime.Now;
            tenantPersonaValidation.Status = command.data.attributes.payload.data.attributes.status;
            tenantPersonaValidation.UrlImages = command.GetPhotos();

            await _tenantPersonalValidationRepository.Add(tenantPersonaValidation);
        }
    }

    public static class TenantPersonaValidationCommandExtension
    {
        public static List<string> GetPhotos(this TenantPersonaValidationCommand command)
        {
            var photos = new List<string>();

            if (command.data.attributes.payload.included.Any())
            {
                command.data.attributes.payload.included.ForEach(inc =>
                {
                    if (!string.IsNullOrWhiteSpace(inc.attributes.frontphotourl))
                        photos.Add(inc.attributes.frontphotourl);

                    if (!string.IsNullOrWhiteSpace(inc.attributes.leftphotourl))
                        photos.Add(inc.attributes.leftphotourl);

                    if (!string.IsNullOrWhiteSpace(inc.attributes.rightphotourl))
                        photos.Add(inc.attributes.rightphotourl);

                    if (!string.IsNullOrWhiteSpace(inc.attributes.centerphotourl))
                        photos.Add(inc.attributes.centerphotourl);
                });
            }

            return photos;
        }
    }
}
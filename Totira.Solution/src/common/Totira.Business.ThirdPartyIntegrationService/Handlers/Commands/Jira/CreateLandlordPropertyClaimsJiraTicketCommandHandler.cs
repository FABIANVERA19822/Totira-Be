using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService;
using Totira.Business.ThirdPartyIntegrationService.Events.Jira;
using Totira.Business.ThirdPartyIntegrationService.Helpers.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    public class CreateLandlordPropertyClaimsJiraTicketCommandHandler : BaseMessageHandler<CreateLandlordPropertyClaimsJiraTicketCommand, CreateLandlordPropertyClaimsJiraTicketEvent>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly JiraOptions _jiraOptions;
        private readonly IRepository<LandlordPropertyClaimValidation, string> _landlordPropertyClaimValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;

        public CreateLandlordPropertyClaimsJiraTicketCommandHandler(
            IRepository<LandlordPropertyClaimValidation, string> landlordPropertyClaimValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<RestClientOptions> restClientOptions,
            IOptions<JiraOptions> jiraOptions,
            ILogger<CreateLandlordPropertyClaimsJiraTicketCommandHandler> logger,
            IQueryRestClient queryRestClient,
            IContextFactory contextFactory,
            IMessageService messageService)
            : base(logger, contextFactory, messageService)
        {
            _landlordPropertyClaimValidationRepository = landlordPropertyClaimValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _restClientOptions = restClientOptions.Value;
            _jiraOptions = jiraOptions.Value;
            _queryRestClient = queryRestClient;
        }

        private static List<Contents> BuildDescriptionTicket(GetPropertyClaimDto properties)
        {
            var contents = new List<Contents>();

            List<string> lines = new List<string>();

            contents.Add(JiraTicketHelper.BuildParagraph("Information about property claims "));

            lines = new List<string>();
            if (properties.Role is not null)
            {
                contents.Add(JiraTicketHelper.BuildParagraph("Information about role "));
                lines.Add($"Role for this property: {properties.Role}");
                contents.Add(JiraTicketHelper.BuildBulletPoints(lines));
            }
            if (string.IsNullOrEmpty(properties.MlsID))
            {
                var MLSId = properties.MlsID;
                lines.Add($"MLS Id: {MLSId}");
            }
            if (string.IsNullOrEmpty(properties.Address))
            {
                var address = properties.Address;
                lines.Add($"Property address: {address}");
            }
            if (string.IsNullOrEmpty(properties.ListingUrl))
            {
                var url = properties.ListingUrl;
                lines.Add($"URL: {url}");
            }
            if (string.IsNullOrEmpty(properties.Unit))
            {
                var unit = properties.Unit;
                lines.Add($"Unit: {unit}");
            }

            contents.Add(JiraTicketHelper.BuildBulletPoints(lines));

            return contents;
        }

        protected override async Task<CreateLandlordPropertyClaimsJiraTicketEvent> Process(IContext context, CreateLandlordPropertyClaimsJiraTicketCommand command)
        {

            var urlLandlordIdentity = $"{_restClientOptions.User}/User/Landlord/{command.LandlordId}/identity-info";
            var landlordIdentity = await _queryRestClient.GetAsync<GetLandlordIdentityInformationDto>(urlLandlordIdentity);

            if (landlordIdentity.Content == null)
            {
                var error = "Landlord has not a Property Claims validation process.";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            var urlPropertyClaims = $"{_restClientOptions.User}/User/Landlord/{command.LandlordId}/PerndingClaimsFromLandlord";
            var propertyClaims = await _queryRestClient.GetAsync<List<GetPropertyClaimDto>>(urlPropertyClaims);

            if (propertyClaims.Content == null)
            {
                var error = "Landlord has not a Property Claims validation process.";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            var basicInfoUrl = $"{_restClientOptions.User}/user/landlord/{command.LandlordId}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetLandlordBasicInformationDto>(basicInfoUrl);

            if (basicInfo == null)
            {
                var error = "Landlord has not Basic Information completed.";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            _logger.LogInformation($"Starting procces for send information of property claim to jira landlord id {command.LandlordId}");
            if (propertyClaims.Content != null && propertyClaims.Content.Any())
            {
                foreach (var claims in propertyClaims.Content)
                {
                    var obj = JiraTicketHelper.BuildBaseTicket(int.Parse(_jiraOptions.PropertyIssueTypeId), $"{basicInfo.Content.FirstName} {basicInfo.Content.LastName}", _jiraOptions.PropertyProjectId, "please validate the documentation attached to the ticket ", new string[] { "Property_Ownership_Check" });
                    List<Contents> contents = BuildDescriptionTicket(claims);

                    obj.fields.description.content = contents.ToArray();

                    var result = await JiraTicketHelper.SendTicketToJira(_jiraOptions, obj);
                    if (landlordIdentity.Content != null && landlordIdentity.Content.IdentityProofs.Any())
                    {
                        foreach (var identity in landlordIdentity.Content.IdentityProofs)
                        {
                            var urlFiles = $"{_restClientOptions.User}/userFiles/{command.LandlordId}/{identity.FileName}";
                            var filesResponse = await _queryRestClient.GetAsync<GetPropertyClaimFilesDto>(urlFiles);

                            foreach (var file in filesResponse.Content.Files)
                            {
                                await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                            }
                        }
                    }

                    var urlFilesCl = $"{_restClientOptions.User}/userFiles/{command.LandlordId}";
                    var filesResponseCl = await _queryRestClient.GetAsync<GetPropertyClaimFilesDto>(urlFilesCl);

                    foreach (var file in filesResponseCl.Content.Files)
                    {
                        await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                    }

                    LandlordPropertyClaimValidation claim = new LandlordPropertyClaimValidation()
                    {
                        Id = result.id,
                        LandlordId = command.LandlordId,
                        Status = "Created",
                        CreatedOn = DateTimeOffset.Now
                    };

                    await _landlordPropertyClaimValidationRepository.Add(claim);
                }
                return new CreateLandlordPropertyClaimsJiraTicketEvent(command.LandlordId, "Request was successful");
            }
            else
            {
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { "Request was not successful" } };
            }
        }
    }
}
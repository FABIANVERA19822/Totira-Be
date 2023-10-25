using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Commands.PropertyClaims;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService;
using Totira.Business.ThirdPartyIntegrationService.Events.Jira;
using Totira.Business.ThirdPartyIntegrationService.Helpers.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;
using File = Totira.Business.ThirdPartyIntegrationService.DTO.UserService.File;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    public class CreateLandlordPropertyClaimsJiraTicketCommandHandler : BaseMessageHandler<CreateLandlordPropertyClaimsJiraTicketCommand, CreateLandlordPropertyClaimsJiraTicketEvent>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly JiraOptions _jiraOptions;
        private readonly IRepository<LandlordPropertyClaimValidation, string> _landlordPropertyClaimValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly IEventBus _bus;

        public CreateLandlordPropertyClaimsJiraTicketCommandHandler(
            IRepository<LandlordPropertyClaimValidation, string> landlordPropertyClaimValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<RestClientOptions> restClientOptions,
            IOptions<JiraOptions> jiraOptions,
            ILogger<CreateLandlordPropertyClaimsJiraTicketCommandHandler> logger,
            IQueryRestClient queryRestClient,
            IContextFactory contextFactory,
            IMessageService messageService,
            IEventBus bus)
            : base(logger, contextFactory, messageService)
        {
            _landlordPropertyClaimValidationRepository = landlordPropertyClaimValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _restClientOptions = restClientOptions.Value;
            _jiraOptions = jiraOptions.Value;
            _queryRestClient = queryRestClient;
            _bus = bus;
        }

        private static List<Contents> BuildDescriptionTicket(GetPendingLandlordClaimsDto properties, out string mlsId)
        {
            var contents = new List<Contents>();

            mlsId = string.Empty;

            List<string> lines = new List<string>();

            contents.Add(JiraTicketHelper.BuildParagraph("Information about property claims "));

            lines = new List<string>();
            if (properties.Role is not null)
            {
                contents.Add(JiraTicketHelper.BuildParagraph("Information about role "));
                lines.Add($"Role for this property: {properties.Role}");
            }
            if (!string.IsNullOrEmpty(properties.MlsID))
            {
                mlsId = properties.MlsID;
                lines.Add($"MLS Id: {mlsId}");
            }
            if (!string.IsNullOrEmpty(properties.Address))
            {
                var address = properties.Address;
                var city = properties.City;
                var unit = !string.IsNullOrEmpty(properties.Unit) ? $"{properties.Unit} - " : string.Empty;
                lines.Add($"Property address: {unit}{address}, {city}");
            }
            if (!string.IsNullOrEmpty(properties.ListingUrl))
            {
                var url = properties.ListingUrl;
                lines.Add($"URL: {url}");
                mlsId = url.Substring(url.Length - 8);
            }

            contents.Add(JiraTicketHelper.BuildBulletPoints(lines));

            return contents;
        }

        protected override async Task<CreateLandlordPropertyClaimsJiraTicketEvent> Process(IContext context, CreateLandlordPropertyClaimsJiraTicketCommand command)
        {
            //Landlord Info
            var urlLandlordIdentity = $"{_restClientOptions.User}/User/Landlord/{command.LandlordId}/identity-info";
            var landlordIdentity = await _queryRestClient.GetAsync<GetLandlordIdentityInformationDto>(urlLandlordIdentity);

            if (landlordIdentity.Content == null)
            {
                var error = "Landlord has not a Property Claims validation process.";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            var urlPropertyClaims = $"{_restClientOptions.User}/User/Landlord/{command.LandlordId}/pendingClaimsFromLandlord";
            var propertyClaims = await _queryRestClient.GetAsync<IEnumerable<GetPendingLandlordClaimsDto>>(urlPropertyClaims);

            if (propertyClaims.Content == null)
            {
                var error = "Landlord has not a Property Claims validation process. ";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            var basicInfoUrl = $"{_restClientOptions.User}/user/landlord/{command.LandlordId}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetLandlordBasicInformationDto>(basicInfoUrl);

            if (basicInfo == null)
            {
                var error = "Landlord has not Basic Information completed. ";
                _logger.LogWarning(error);
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { error } };
            }

            _logger.LogInformation($"Starting procces for send information of property claim to jira landlord id {command.LandlordId}");
            if (propertyClaims.Content != null && propertyClaims.Content.Any())
            {
                foreach (var claim in propertyClaims.Content)
                {
                    var obj = JiraTicketHelper.BuildBaseTicket(int.Parse(_jiraOptions.PropertyIssueTypeId), $"{basicInfo.Content.FirstName} {basicInfo.Content.LastName}", _jiraOptions.PropertyProjectId, "please validate the documentation attached to the ticket ", new string[] { "Property_Ownership_Check" });
                    List<Contents> contents = BuildDescriptionTicket(claim, out string mlsId);

                    obj.fields.description.content = contents.ToArray();
                    obj.fields.customfield_10060 = mlsId;
                    var result = await JiraTicketHelper.SendTicketToJira(_jiraOptions, obj);

                    if (landlordIdentity.Content != null && landlordIdentity.Content.IdentityProofs.Any())
                    {
                        foreach (var identity in landlordIdentity.Content.IdentityProofs)
                        {
                            var urlFiles = $"{_restClientOptions.User}/userFiles/landlords/{command.LandlordId}/identityFiles/{identity.FileName}";
                            var file = (await _queryRestClient.GetAsync<File>(urlFiles)).Content;

                            await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                        }
                    }

                    if (claim.OwnershipProofs.Any())
                    {
                        foreach (var claimFile in claim.OwnershipProofs)
                        {
                            var urlFiles = $"{_restClientOptions.User}/UserFiles/landlords/{command.LandlordId}/claimFiles/{claimFile.FileName}";
                            var file = (await _queryRestClient.GetAsync<File>(urlFiles)).Content;

                            await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                        }
                    }

                    await AddLandLordPropertyClaim(result.id, command.LandlordId, claim.Id, basicInfo.Content.Email, claim.Address);

                    await UpdateClaimJiraTicketCreation(claim.Id);
                }
                return new CreateLandlordPropertyClaimsJiraTicketEvent(command.LandlordId, "Request was successful");
            }
            else
            {
                return new CreateLandlordPropertyClaimsJiraTicketEvent() { Errors = new List<string> { "Request was not successful" } };
            }
        }

        private async Task AddLandLordPropertyClaim(string jiraId, Guid landLordId, Guid claimId, string email, string address)
        {
            LandlordPropertyClaimValidation propertyClaim = new LandlordPropertyClaimValidation()
            {
                Id = jiraId,
                LandlordId = landLordId,
                ClaimId = claimId,
                Email = email,
                Status = "Created",
                CreatedOn = DateTimeOffset.Now,
                Address = address
            };

            await _landlordPropertyClaimValidationRepository.Add(propertyClaim);
        }

        private async Task UpdateClaimJiraTicketCreation(Guid claimId)
        {
            var request = new UpdateClaimJiraTicketCreationCommand(claimId);
            var userContext = _contextFactory.Create(string.Empty, Guid.Empty);

            await _bus.PublishAsync(userContext, request);
        }
    }
}
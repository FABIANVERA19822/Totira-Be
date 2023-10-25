using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService;
using Totira.Business.ThirdPartyIntegrationService.Helpers.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
	public class CreateGroupProfileInterestJiraticketCommandHandler : IMessageHandler<CreateGroupProfileInterestJiraticketCommand>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IEncryptionHandler _encryptHandler;
        private readonly FrontendSettings _frontendSettings;
        private readonly JiraOptions _jiraOptions;
        private readonly ILogger<CreateProfileInterestJiraTicketCommandHandler> _logger;
        public CreateGroupProfileInterestJiraticketCommandHandler(
            IOptions<RestClientOptions> restClientOptions,
            IOptions<JiraOptions> jiraOptions,
            IOptions<FrontendSettings> frontendSettings,
            ILogger<CreateProfileInterestJiraTicketCommandHandler> logger,
            IQueryRestClient queryRestClient,
            IEncryptionHandler encryptHandler)
		{
            _restClientOptions = restClientOptions.Value;
            _jiraOptions = jiraOptions.Value;
            _frontendSettings = frontendSettings.Value;
            _queryRestClient = queryRestClient;
            _encryptHandler = encryptHandler;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, CreateGroupProfileInterestJiraticketCommand command)
        {
            var GroupApplicationSummaryUrl = $"{_restClientOptions.User}/user/tenant/GroupApplicationSummary/{command.TenantId}";
            var GroupApplicationSummary = await _queryRestClient.GetAsync<GetTenantGroupApplicationSummaryDto>(GroupApplicationSummaryUrl);

            var encryptedAccessCode = _encryptHandler.EncryptString(command.ProfileAccessCode.ToString());

            var shareProfileUrl = $"{_restClientOptions.User}/user/CheckTenantGroupShareProfileForCheckCode/{command.TenantId}/{command.ProfileAccessCode}/{command.AgentEmail}";
            var shareProfile = await _queryRestClient.GetAsync<GetTenantShareProfileForCheckCodeAndEmailDto>(shareProfileUrl);

            List<GetTenantBasicInfoAndContactInfoDto> BasicInfoAndContactInfo = new List<GetTenantBasicInfoAndContactInfoDto>();

            string mainApplicantName = string.Empty;

            if (GroupApplicationSummary?.Content?.MainApplicant?.Id != null)
            {
                var mainApplicant = await GetTenantBasicInfoAndContactInfo(GroupApplicationSummary.Content.MainApplicant.Id);
                mainApplicant.Role = "MainApplicant";
                BasicInfoAndContactInfo.Add(mainApplicant);

                mainApplicantName = $"{mainApplicant.BasicInformation.FirstName} {mainApplicant.BasicInformation.LastName}";
            }

            if (GroupApplicationSummary?.Content?.Guarantor?.Id != null)
            {
                var guarantor = await GetTenantBasicInfoAndContactInfo(GroupApplicationSummary.Content.Guarantor.Id);
                guarantor.Role = "Guarantor";

                if(guarantor?.BasicInformation?.FirstName != null)
                BasicInfoAndContactInfo.Add(guarantor);
            }

            if (GroupApplicationSummary?.Content?.CoApplicants != null)
            {
                foreach (var CoApplicant in GroupApplicationSummary.Content.CoApplicants)
                {
                    if(CoApplicant.Id != null)
                    {
                        var coapplicant = await GetTenantBasicInfoAndContactInfo(CoApplicant.Id);
                        coapplicant.Role = "CoApplicant";
                        BasicInfoAndContactInfo.Add(coapplicant);
                    }
                }

            }


            _logger.LogInformation($"Starting show interes in tenant id {command.TenantId}");
            var obj = JiraTicketHelper.BuildBaseTicket(int.Parse(_jiraOptions.AgentIssueTypeId), $"Interest in Group profile with Main Applicant {mainApplicantName}", _jiraOptions.AgentProjectId, " Hello one agent is interested in this profile please follow the process  ");

            
            var link = $"{_frontendSettings.Url}/user?tenantId={command.TenantId}";
            List<Contents> contents = BuildDescriptionTicket(BasicInfoAndContactInfo, shareProfile.Content, command.ProfileAccessCode.ToString(), command.AgentEmail, link);

            obj.fields.description.content = contents.ToArray();

            var result = await JiraTicketHelper.SendTicketToJira(_jiraOptions, obj);
        }

        private async Task<GetTenantBasicInfoAndContactInfoDto> GetTenantBasicInfoAndContactInfo(Guid id)
        {
            var basicInfoUrl = $"{_restClientOptions.User}/user/tenant/{id}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetTenantBasicInformationDto>(basicInfoUrl);


            var contactInfoUrl = $"{_restClientOptions.User}/user/tenant/contactinfo/{id}";
            var contactInfo = await _queryRestClient.GetAsync<GetTenantContactInformationDto>(contactInfoUrl);

            return new GetTenantBasicInfoAndContactInfoDto()
            {
                BasicInformation = basicInfo.Content,
                ContactInformation = contactInfo.Content
            };
        }
        private static List<Contents> BuildDescriptionTicket(List<GetTenantBasicInfoAndContactInfoDto> basicInfoAndContactInfo, GetTenantShareProfileForCheckCodeAndEmailDto profile, string code ,string agentEmail, string link)
        {
            var contents = new List<Contents>();

            if (basicInfoAndContactInfo != null)
            {
                contents.Add(JiraTicketHelper.BuildParagraph("Information about the tenants"));

                foreach (var item in basicInfoAndContactInfo)
                {

                    var tenantFullName = $"{item?.BasicInformation?.FirstName} {item?.BasicInformation?.LastName}";
           
                    var tenantPhoneNumber = $"{item?.ContactInformation?.PhoneNumber?.CountryCode} {item?.ContactInformation?.PhoneNumber?.Number}";



                    List<string> lines = new List<string>();
                    lines.Add($"Tenant full name: {tenantFullName}");
                    lines.Add($"Tenant Role: {item?.Role}");
                    lines.Add($"Tenant email: {item?.ContactInformation?.Email}");
                    lines.Add($"Tenant phone number: {tenantPhoneNumber}");



                    contents.Add(JiraTicketHelper.BuildBulletPoints(lines));
                }
            }

            contents.Add(JiraTicketHelper.BuildParagraph("Information about the Profile"));

     

            List<string> profilelines = new List<string>();
            profilelines.Add($"Profile link: {link}");
            profilelines.Add($"Profile access code: {code}");

            contents.Add(JiraTicketHelper.BuildBulletPoints(profilelines));



            contents.Add(JiraTicketHelper.BuildParagraph("Information about the agent "));

            List<string> agentLines = new List<string>();
            agentLines.Add($"Role of the agent/landlord (optional): {profile?.TypeOfContact}");
            agentLines.Add($"Email of the agent/landlord: {agentEmail}");
            contents.Add(JiraTicketHelper.BuildBulletPoints(agentLines));


            return contents;
        }
    }
}

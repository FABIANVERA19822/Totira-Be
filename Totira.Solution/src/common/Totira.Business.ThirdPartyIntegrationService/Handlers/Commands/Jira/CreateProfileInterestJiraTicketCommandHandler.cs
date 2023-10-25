using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
    public class CreateProfileInterestJiraTicketCommandHandler : IMessageHandler<CreateProfileInterestJiraTicketCommand>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IEncryptionHandler _encryptHandler;
        private readonly FrontendSettings _frontendSettings;
        private readonly JiraOptions _jiraOptions;
        private readonly ILogger<CreateProfileInterestJiraTicketCommandHandler> _logger;
        public CreateProfileInterestJiraTicketCommandHandler(
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

        public async Task HandleAsync(IContext context, Either<Exception, CreateProfileInterestJiraTicketCommand> command)
        {
            await command.MatchAsync(async cmd => {
                var basicInfoUrl = $"{_restClientOptions.User}/user/tenant/{cmd.TenantId}/basic-info";
                var basicInfo = await _queryRestClient.GetAsync<GetTenantBasicInformationDto>(basicInfoUrl);

                var encryptedAccessCode = _encryptHandler.EncryptString(cmd.ProfileAccessCode.ToString());

                var shareProfileUrl = $"{_restClientOptions.User}/user/tenant/{cmd.TenantId}/{encryptedAccessCode}/{cmd.AgentEmail}/shareProfile";
                var shareProfile = await _queryRestClient.GetAsync<GetTenantShareProfileDto>(shareProfileUrl);

                var contactInfoUrl = $"{_restClientOptions.User}/user/tenant/contactinfo/{cmd.TenantId}";
                var contactInfo = await _queryRestClient.GetAsync<GetTenantContactInformationDto>(contactInfoUrl);


                _logger.LogInformation($"Starting show interes in tenant id {cmd.TenantId}");
                var obj = JiraTicketHelper.BuildBaseTicket(int.Parse(_jiraOptions.AgentIssueTypeId), $"Interest in tenant {basicInfo.Content.FirstName} {basicInfo.Content.LastName}", _jiraOptions.AgentProjectId, " Hello one agent is interested in this profile please follow the process  ");

                
                var link = $"{_frontendSettings.Url}/user?tenantId={cmd.TenantId}";
                List<Contents> contents = BuildDescriptionTicket(basicInfo.Content, shareProfile.Content, contactInfo.Content, cmd.ProfileAccessCode.ToString(), cmd.AgentEmail, link);

                obj.fields.description.content = contents.ToArray();

                var result = await JiraTicketHelper.SendTicketToJira(_jiraOptions, obj);
            }, ex => throw ex);

        }

        private static List<Contents> BuildDescriptionTicket(GetTenantBasicInformationDto basicInfo, GetTenantShareProfileDto profile, GetTenantContactInformationDto contactInfo, string code, string agentEmail, string link)
        {
            var contents = new List<Contents>();
    

            contents.Add(JiraTicketHelper.BuildParagraph("Information about the tenant "));



            var tenantFullName = $"{basicInfo?.FirstName} {basicInfo?.LastName}";
            
            var tenantPhoneNumber = $"{contactInfo?.PhoneNumber?.CountryCode} {contactInfo?.PhoneNumber?.Number}";



            List<string> lines = new List<string>();
            lines.Add($"Tenant full name: {tenantFullName}");
            lines.Add($"Tenant email: {contactInfo?.Email}");
            lines.Add($"Tenant phone number: {tenantPhoneNumber}");
            lines.Add($"Tenant profile link: {link}");
            lines.Add($"Profile code: {code}");

            

            contents.Add(JiraTicketHelper.BuildBulletPoints(lines));


            contents.Add(JiraTicketHelper.BuildParagraph("Information about the agent "));

            List<string> agentLines = new List<string>();

            agentLines.Add($"Role of the agent/landlord (optional): {profile?.TypeOfContact}");
            agentLines.Add($"Email of the agent/landlord: {agentEmail}");
            contents.Add(JiraTicketHelper.BuildBulletPoints(agentLines));


            return contents;
        }
    }
}

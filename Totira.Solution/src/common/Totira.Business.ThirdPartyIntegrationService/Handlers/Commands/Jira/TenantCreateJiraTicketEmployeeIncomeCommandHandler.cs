using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService;
using Totira.Business.ThirdPartyIntegrationService.Helpers.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    public class TenantCreateJiraTicketEmployeeIncomeCommandHandler : IMessageHandler<TenantEmployeeAndIncomeTicketJiraCommand>
    {

        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly JiraOptions _jiraOptions;
        private readonly IRepository<TenantEmployeeInconmeValidation, string> _tenantJiraValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly ILogger<TenantCreateJiraTicketEmployeeIncomeCommandHandler> _logger;

        public TenantCreateJiraTicketEmployeeIncomeCommandHandler(
            IRepository<TenantEmployeeInconmeValidation, string> tenantJiraValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<RestClientOptions> restClientOptions,
            IOptions<JiraOptions> jiraOptions,
            ILogger<TenantCreateJiraTicketEmployeeIncomeCommandHandler> logger,
            IQueryRestClient queryRestClient)
        {

            _tenantJiraValidationRepository = tenantJiraValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _restClientOptions = restClientOptions.Value;
            _jiraOptions = jiraOptions.Value;
            _queryRestClient = queryRestClient;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, TenantEmployeeAndIncomeTicketJiraCommand command)
        {
            var urlIncomes = $"{_restClientOptions.User}/user/tenant/{command.TenantId}/incomes";
            var incomes = await _queryRestClient.GetAsync<GetTenantEmployeeIncomesDto>(urlIncomes);

            if (incomes == null)
                return;

            var basicInfoUrl = $"{_restClientOptions.User}/user/tenant/{command.TenantId}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetTenantBasicInformationDto>(basicInfoUrl);



            _logger.LogInformation($"Starting procces for send information of employeement and income to jira tenant id {command.TenantId}");
            var obj = JiraTicketHelper.BuildBaseTicket(int.Parse(_jiraOptions.IssueTypeId), $"{basicInfo.Content.FirstName} {basicInfo.Content.LastName}", _jiraOptions.ProjectId, "please validate the documentation attached to the ticket ", new string[] { "Employment_and_Info_check" });
            List<Contents> contents = BuildDescriptionTicket(incomes);

            obj.fields.description.content = contents.ToArray();

            var result = await JiraTicketHelper.SendTicketToJira(_jiraOptions, obj);

            if (incomes.Content.StudyDetails is not null)
                foreach (var study in incomes.Content.StudyDetails)
                {
                    var urlFiles = $"{_restClientOptions.User}/userFiles/{command.TenantId}/{study.StudyId}";
                    var filesResponse = await _queryRestClient.GetAsync<GetIncomeFilesDto>(urlFiles);

                    foreach (var file in filesResponse.Content.Files)
                        await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                }

            foreach (var inc in incomes.Content.CurrentEmployements)
            {
                var urlFiles = $"{_restClientOptions.User}/userFiles/{command.TenantId}/{inc.IncomeId}";
                var filesResponse = await _queryRestClient.GetAsync<GetIncomeFilesDto>(urlFiles);

                foreach (var file in filesResponse.Content.Files)
                {
                    await JiraTicketHelper.AttachFileToJira(_jiraOptions, result.id, file.FileName, file.Extension, file.Content);
                }
            }


            TenantEmployeeInconmeValidation income = new TenantEmployeeInconmeValidation()
            {
                Id = result.id,
                TenantId = command.TenantId,
                Status = "Created",
                CreatedOn = DateTimeOffset.Now
            };

            await _tenantJiraValidationRepository.Add(income);

            Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == command.TenantId);
            var verifications = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();

            if (verifications == null)
            {
                // Create TenantVerifiedProfile
                var tenantProfile = TenantVerifiedProfile.CreateVerifiedProfile(
                   command.TenantId,
                    false,
                    false,
                    true,
                    false);
                await _tenantVerifiedProfileRepository.Add(tenantProfile);
            }
        }

        private static List<Contents> BuildDescriptionTicket(QueryResponse<GetTenantEmployeeIncomesDto> incomes)
        {
            var contents = new List<Contents>();


            List<string> lines = new List<string>();
            if (incomes.Content.StudyDetails is not null && incomes.Content.StudyDetails.Any()) {
                contents.Add(JiraTicketHelper.BuildParagraph("Information about student "));

                foreach (var study in incomes.Content.StudyDetails)
                {
                    lines.Add($"University or institution they have enrolled with {study.UniversityOrInstitute}");
                    lines.Add($"Degree they have enrolled in  {study.Degree}");
                }
                contents.Add(JiraTicketHelper.BuildBulletPoints(lines));
            }

            if (incomes.Content.CurrentEmployements.Any())
            {
                contents.Add(JiraTicketHelper.BuildParagraph("Information about current employments "));


                foreach (var currentEmployements in incomes.Content.CurrentEmployements)
                {
                    var currentCompanyName = currentEmployements.CompanyOrganizationName;
                    var currentPosition = currentEmployements.Position;
                    var currentMonthlyIncome = currentEmployements.MonthlyIncome;
                    var currentStartDate = currentEmployements.StartDate;

                    lines = new List<string>();
                    lines.Add($"Current company name {currentCompanyName}");
                    lines.Add($"Current position {currentPosition}");
                    lines.Add($"Current monthly income {currentMonthlyIncome}");
                    lines.Add($"Start date in the company {currentStartDate}");

                    contents.Add(JiraTicketHelper.BuildBulletPoints(lines));
                }

                if (incomes.Content.PastEmployments.Any())
                {
                    contents.Add(JiraTicketHelper.BuildParagraph("Information about past employments "));
                    foreach (var passEmployements in incomes.Content.PastEmployments)
                    {
                        var passCompanyName = passEmployements.CompanyOrganizationName;
                        var passPosition = passEmployements.Position;
                        var passMonthlyIncome = passEmployements.Period;

                        List<string> passInfo = new List<string>();

                        passInfo.Add($"Past company name {passCompanyName}");
                        passInfo.Add($"Past position {passPosition}");
                        passInfo.Add($"Past monthly income {passMonthlyIncome}");

                        contents.Add(JiraTicketHelper.BuildBulletPoints(passInfo));

                    }
                }

            }



            return contents;
        }
    }




}

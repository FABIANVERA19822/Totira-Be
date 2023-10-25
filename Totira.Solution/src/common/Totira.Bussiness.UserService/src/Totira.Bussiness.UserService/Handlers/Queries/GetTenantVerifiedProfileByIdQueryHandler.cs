using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Handlers.Queries
{

    public class GetTenantVerifiedProfileByIdQueryHandler : IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;

        public GetTenantVerifiedProfileByIdQueryHandler(           
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions
            )
        {
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
        }


        public async Task<GetTenantVerifiedbyProfileDto> HandleAsync(QueryTenantVerifiedProfileById query)
        {

            #region GetThirdPartyValidations  

            var infoCertn = await _queryRestClient.GetAsync<GetCertnApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Certn/applicants/{query.Id.ToString()}");

            var infoPersona = await GetPersonaValidation(query.Id);

            var infoJira = await GetJiraValidations(query.Id);

            #endregion
            
            var identityInfo = infoPersona != null && infoPersona.TenantId != Guid.Empty ? infoPersona.Status : string.Empty;
            IncomeValidation financialInfo = GetIncomeValidation(query, infoJira);
            BackgroundCheck backgroundCheck;
            CreditScore creditScore;

            GetCertnValidations(infoCertn, out backgroundCheck, out creditScore);

            var result =
                    infoCertn != null || infoJira != null || infoPersona != null ?
                        new GetTenantVerifiedbyProfileDto(query.Id, identityInfo, financialInfo, creditScore, backgroundCheck) :
                        new GetTenantVerifiedbyProfileDto(query.Id);

            return result;
        }

        private async Task<GetJiraEmployementTicketDto> GetJiraValidations(Guid id)
        {
            GetJiraEmployementTicketDto result = new GetJiraEmployementTicketDto();
             var thirdPartyJiraEmployementResponse = await _queryRestClient.GetAsync<GetJiraEmployementTicketDto>($"{_restClientOptions.ThirdPartyIntegration}/Jira/applicants/{id}");
            if (thirdPartyJiraEmployementResponse.StatusCode is HttpStatusCode.OK)
            {
                result = thirdPartyJiraEmployementResponse.Content;
            }
            else
            {
                result = null;
            }
            return result;
        }

        private async Task<GetPersonaApplicationDto> GetPersonaValidation(Guid id)
        {
            GetPersonaApplicationDto result = new GetPersonaApplicationDto();
            var thirdPartyPersonaResponse = await _queryRestClient.GetAsync<GetPersonaApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Persona/applicants/{id}");
            if (thirdPartyPersonaResponse.StatusCode is HttpStatusCode.OK)
            {
                result = thirdPartyPersonaResponse.Content;
            }
            else
            {
                result = null;
            }
            return result;
        }

        private static void GetCertnValidations(QueryResponse<GetCertnApplicationDto>? infoCertn, out BackgroundCheck backgroundCheck, out CreditScore creditScore)
        {
            if (infoCertn.Content != null)
            {
                var responseCertn = infoCertn is not null ? JObject.Parse(infoCertn.Content.JsonResponse) : null;
                if (responseCertn is not null)
                {
                    backgroundCheck = new BackgroundCheck()
                    {
                        Status = infoCertn != null ? infoCertn.Content.StatusSoftCheck : string.Empty,
                        CriminalConvictionStatus = responseCertn.Count != 0 ? responseCertn.SelectToken("risk_result.scan_status.criminal_scan").ToString() : string.Empty,
                        FraudStatus = responseCertn.Count != 0 ? responseCertn.SelectToken("risk_result.scan_status.fraud_scan").ToString() : string.Empty,
                        CourtRecordsStatus = responseCertn.Count != 0 ? responseCertn.SelectToken("risk_result.scan_status.public_court_records").ToString() : string.Empty,
                        SexOffenderStatus = responseCertn.Count != 0 ? responseCertn.SelectToken("risk_result.scan_status.sex_offender_scan").ToString() : string.Empty
                    };

                    creditScore = new CreditScore()
                    {
                        Status = infoCertn != null ? infoCertn.Content.StatusEquifax : string.Empty,
                        Score = responseCertn.Count != 0 ? responseCertn.SelectToken("equifax_result.credit_score").ToString() : string.Empty
                    };
                    if (creditScore.Score == string.Empty)
                        creditScore.Score = "650";
                }
                else
                {
                    backgroundCheck = new BackgroundCheck();
                    creditScore = new CreditScore();
                }
            }
            else
            {
                backgroundCheck = new BackgroundCheck();
                creditScore = new CreditScore();
            }

        }

        private static IncomeValidation GetIncomeValidation(QueryTenantVerifiedProfileById query, GetJiraEmployementTicketDto infoJira)
        {
            var EmployeeIncomes = new List<EmployeeIncome>();
            foreach (CurrentEmploymentDto employee in query.IncomesDto.CurrentEmployements)
            {
                EmployeeIncomes.Add(new EmployeeIncome()
                {
                    MonthlySalary = employee.MonthlyIncome,
                    Role =  employee.Position,
                    CompanyName = employee.CompanyOrganizationName
                });

            }

            var financialInfo = new IncomeValidation()
            {
                Status = infoJira != null && infoJira.TenantId != Guid.Empty ? infoJira.Status : string.Empty,
                Employee = EmployeeIncomes,
                Student = new Student()
                {
                    Degree = query.IncomesDto.StudyDetails.FirstOrDefault()?.Degree ?? string.Empty,
                    InstitutionName = query.IncomesDto.StudyDetails.FirstOrDefault()?.UniversityOrInstitute ?? string.Empty
                }  
            };
            return financialInfo;
        }       

    }
}

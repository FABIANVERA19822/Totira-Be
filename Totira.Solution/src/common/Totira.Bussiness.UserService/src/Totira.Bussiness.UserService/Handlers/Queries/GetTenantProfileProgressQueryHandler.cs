using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Net;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantProfileProgressQueryHandler : IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IRepository<TenantApplicationDetails, Guid> _applicationDetailsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _basicInfoRepository;
        private readonly IRepository<TenantContactInformation, Guid> _contactInfoRepository;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _employeeIncomesRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _applicationRequestRepository;

        public GetTenantProfileProgressQueryHandler(IRepository<TenantApplicationDetails, Guid> applicationDetailsRepository,
                                                    IRepository<TenantBasicInformation, Guid> basicInfoRepository,
                                                    IRepository<TenantContactInformation, Guid> contactInfoRepository,
                                                    IRepository<TenantEmployeeIncomes, Guid> employeeIncomesRepository,
                                                    IRepository<TenantApplicationRequest, Guid> applicationRequestRepository,
                                                    IQueryRestClient queryRestClient,
                                                    IOptions<RestClientOptions> restClientOptions
                                                    )
        {
            _applicationDetailsRepository = applicationDetailsRepository;
            _basicInfoRepository = basicInfoRepository;
            _contactInfoRepository = contactInfoRepository;
            _employeeIncomesRepository = employeeIncomesRepository;
            _applicationRequestRepository = applicationRequestRepository;
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
        }

        public async Task<Dictionary<string, int>> HandleAsync(QueryTenantProfileProgressByTenantId query)
        {
            Expression<Func<TenantContactInformation, bool>> contactExpression = x => x.TenantId == query.TenantId;
            Expression<Func<TenantApplicationDetails, bool>> applicationDetailExpression = x => x.TenantId == query.TenantId;
            Expression<Func<TenantApplicationRequest, bool>> requestExpression = x => x.TenantId == query.TenantId
                                                                                   || x.Guarantor.Id == query.TenantId
                                                                                   || x.Coapplicants.Any(c => c.Id == query.TenantId);

            var request = (await _applicationRequestRepository.Get(requestExpression)).FirstOrDefault();
            var application = await (_applicationDetailsRepository.Get(applicationDetailExpression));
            var basic = await (_basicInfoRepository.GetByIdAsync(query.TenantId));
            var contact = await (_contactInfoRepository.Get(contactExpression));

            var officialID = await GetPersonaValidation(query);

            var employ = await (_employeeIncomesRepository.GetByIdAsync(query.TenantId));

            Dictionary<string, int> result = new Dictionary<string, int>
            {
                { TenantProfileSectionsEnum.Application.GetEnumDescription(),
                    (application.MaxBy(ap => ap.CreatedOn).ApplicationComplete()||request.ApplicationRequestComplete()) ? 10 : 0},
                { TenantProfileSectionsEnum.Basic.GetEnumDescription(),basic.BasicComplete() ? 10 : 0},
                { TenantProfileSectionsEnum.Contact.GetEnumDescription(),contact.FirstOrDefault().ContactComplete() ? 20 : 0},
                { TenantProfileSectionsEnum.Employment.GetEnumDescription(),employ.EmploymentComplete() ? 30 : 0},
                { TenantProfileSectionsEnum.OfficialID.GetEnumDescription(),officialID.OfficialIDComplete()?30:0}
            };

            return result;
        }
        private async Task<GetPersonaApplicationDto> GetPersonaValidation(QueryTenantProfileProgressByTenantId query)
        {
            GetPersonaApplicationDto result = new GetPersonaApplicationDto();
            var thirdPartyPersonaResponse = await _queryRestClient.GetAsync<GetPersonaApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Persona/applicants/{query.TenantId}");
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
    }
}

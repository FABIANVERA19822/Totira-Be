
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using Totira.Support.ThirdPartyIntegration.Certn.Constants.Certn;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetVerificationsProcessStatusByIdQueryHandler : IQueryHandler<QueryVerificationsProcessStatusById, string>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _getTenantApplicationDetailsRepository;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        public GetVerificationsProcessStatusByIdQueryHandler
            (
            IRepository<TenantApplicationDetails, Guid> getTenantApllicationDetailsRepository,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions
            )
        {
            _restClientOptions = restClientOptions.Value;
            _getTenantApplicationDetailsRepository = getTenantApllicationDetailsRepository;
            _queryRestClient = queryRestClient;
        }


        public async Task<string> HandleAsync(QueryVerificationsProcessStatusById query)
        {

            var tenantApplicationDetails =
               await _getTenantApplicationDetailsRepository.Get(t => t.TenantId == query.TenantId);
            if (!tenantApplicationDetails.Any())
            {
                return $"Tenant {query.TenantId} dont have any application details";
            }

            var result = await GetCertnResult(_restClientOptions.ThirdPartyIntegration, query.TenantId.ToString());


            if (result.StatusCode != System.Net.HttpStatusCode.Conflict)
            {
                tenantApplicationDetails.FirstOrDefault().IsVerificationsRequested = true;
                await _getTenantApplicationDetailsRepository.Update(tenantApplicationDetails.FirstOrDefault());
                if ((result.Content.StatusEquifax == CertnStatus.Returned || result.Content.StatusEquifax == CertnStatus.Complete)
                     && (result.Content.StatusSoftCheck == CertnStatus.Returned || result.Content.StatusSoftCheck == CertnStatus.Complete))
                {
                    return "Verified";
                }
                else
                {
                    return "Under Revision";
                }
            }
            else
            {
                return "Not Requested";
            }
        }

        private async Task<QueryResponse<GetCertnApplicationDto>> GetCertnResult(string url, string tenantid)
        {
            return await _queryRestClient.GetAsync<GetCertnApplicationDto>($"{url}/Certn/applicants/{tenantid}");
        }
    }
}

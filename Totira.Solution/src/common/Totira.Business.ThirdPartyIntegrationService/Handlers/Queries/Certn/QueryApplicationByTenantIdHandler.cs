using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Totira.Business.Integration.Certn.Interfaces;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Certn;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Queries;
using Totira.Support.ThirdPartyIntegration.Certn.Constants.Certn;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Certn
{
    public class QueryApplicationByTenantIdHandler : IQueryHandler<QueryApplicationByTenantId, GetCertnApplicationDto>
    {
        private readonly IRepository<TenantApplications, string> _tenantApplicationsRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly ICertnHandler _certnHandler;
        private readonly ILogger<QueryApplicationByTenantIdHandler> _logger;
        public QueryApplicationByTenantIdHandler(
            IRepository<TenantApplications, string> tenantApplicationsRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            ICertnHandler certnHandler,
            ILogger<QueryApplicationByTenantIdHandler> logger)
        {
            _tenantApplicationsRepository = tenantApplicationsRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _certnHandler = certnHandler;
            _logger = logger;
        }

        public async Task<GetCertnApplicationDto> HandleAsync(QueryApplicationByTenantId query)
        {
            Expression<Func<TenantApplications, bool>> expression = (p => p.Id == query.TenantId);

            var info = (await _tenantApplicationsRepository.Get(expression)).FirstOrDefault();

            var result1 =
                info != null ?
                    new TenantApplications(info.Id, info.Applications, info.CreatedOn) :
                    new TenantApplications();


            var tmpApplicant = Guid.Empty;
            GetCertnApplicationDto getCertnApplicationDto = null;

            if (result1.Applications.Count == 0)
            {
                _logger.LogWarning("Tenant has not a Certn validation process.");
                return getCertnApplicationDto;
            }

            // invoke CERTN API
            tmpApplicant = result1.Applications[0].ApplicantId;
            var gApplicantId = tmpApplicant.ToString();
            var response = await _certnHandler.GetAsync($"applicants/{gApplicantId}", new { }, true);
            if (response is not null)
            {
                getCertnApplicationDto = new GetCertnApplicationDto(gApplicantId, CertnStatus.Returned, CertnStatus.Returned, response);
                JObject result = JObject.Parse(response);

                var applicationId = (string?)result.SelectToken("application.id");
                var statusRiskResult = (string?)result.SelectToken("risk_result.status");
                var statusEquifaxResult = (string?)result.SelectToken("equifax_result.status");
                if (info.Applications[0].StatusEquifax != statusEquifaxResult || info.Applications[0].StatusSoftCheck != statusRiskResult)
                {
                    result1.Applications.RemoveAt(0);
                    var application = TenantApplication.UpdateSoftCheck(
                        applicationId,
                        string.IsNullOrEmpty(gApplicantId) ? Guid.Empty : Guid.Parse(gApplicantId),
                        statusRiskResult,
                        statusEquifaxResult,
                        response);
                    result1.Applications.Add(application);

                    await _tenantApplicationsRepository.Update(result1);
                }


                if ((statusEquifaxResult == CertnStatus.Returned && statusRiskResult == CertnStatus.Returned) ||
                        (statusEquifaxResult == CertnStatus.RiskStatusNone && statusRiskResult == CertnStatus.Returned))
                {
                    var guidTenantId = Guid.Parse(query.TenantId);
                    var verifications = (await _tenantVerifiedProfileRepository.Get((p => p.TenantId == guidTenantId))).FirstOrDefault();
                    if (verifications != null)
                    {
                        verifications.Certn = true;
                        verifications.UpdatedOn = DateTime.UtcNow;
                        await _tenantVerifiedProfileRepository.Update(verifications);
                    }
                    else
                    {
                        var tenantProfile = TenantVerifiedProfile.CreateVerifiedProfile(
                                          guidTenantId,
                                          true,
                                          false,
                                          true,
                                          false);
                        await _tenantVerifiedProfileRepository.Add(tenantProfile);
                    }


                }
            }
            return getCertnApplicationDto;
        }
    }
}

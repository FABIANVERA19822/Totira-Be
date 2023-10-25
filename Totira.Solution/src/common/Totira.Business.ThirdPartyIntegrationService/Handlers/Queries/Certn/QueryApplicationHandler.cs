using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Certn;
using Totira.Support.Application.Queries;
using Totira.Support.ThirdPartyIntegration.Certn.Constants.Certn;
using static Totira.Business.ThirdPartyIntegrationService.Queries.Certn.QueryApplication;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Certn
{
    public class QueryApplicationHandler : IQueryHandler<QueryApplication, ListTenantApplicationDto>
    {
        private readonly IRepository<TenantApplications, string> _tenantApplicationsRepository;
        private readonly ILogger<QueryApplicationHandler> _logger;

        public QueryApplicationHandler(IRepository<TenantApplications, string> tenantApplicationsRepository,
            ILogger<QueryApplicationHandler> logger)
        {
            _tenantApplicationsRepository = tenantApplicationsRepository;
            _logger = logger;
        }

        public async Task<ListTenantApplicationDto> HandleAsync(QueryApplication query)
        {

            Expression<Func<TenantApplications, bool>> expression = ta => ta.Applications.Where(a => a.StatusEquifax != CertnStatus.Complete || a.StatusSoftCheck != CertnStatus.Complete).Any(); 

            var info = (await _tenantApplicationsRepository.Get(expression));

            ListTenantApplicationDto tenantApplicationDto = new ListTenantApplicationDto();
            tenantApplicationDto.Count = info.Count();

            if (info.Any())
            {
                var paggingData = info.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();                

                var data =  new List<TenantApplicationDto>();

                paggingData.ForEach(ap => { 
                    data.Add(new TenantApplicationDto(ap.Id, ap.Applications[0].ApplicantId, ap.Applications[0].StatusSoftCheck, ap.Applications[0].StatusEquifax, ap.Applications[0].Response, ap.Applications[0].CreatedOn));
                
                });

                tenantApplicationDto.TenantApplications = SortingApplications(data, query.SortBy);
            } else
            {
                tenantApplicationDto.TenantApplications = new List<TenantApplicationDto>();
                _logger.LogWarning("Tenant has not a Certn validation process.");
            }
            return tenantApplicationDto;
        }

        #region Helper Methods
        private List<TenantApplicationDto> SortingApplications(IEnumerable<TenantApplicationDto> applications, EnumApplicantSortBy? sortBy)
        {
            switch (sortBy)
            {
                case EnumApplicantSortBy.Id:
                    applications = applications.OrderByDescending(s => s.Id);
                    break;
                case EnumApplicantSortBy.ApplicantId:
                    applications = applications.OrderBy(s => s.ApplicantId);
                    break;
                case EnumApplicantSortBy.Status:
                    applications = applications.OrderBy(s => s.StatusSoftCheck);
                    break;
                case EnumApplicantSortBy.CreatedOn:
                    applications = applications.OrderByDescending(s => s.CreatedOn);
                    break;
                default:
                    applications = applications.OrderByDescending(s => s.CreatedOn);
                    break;
            }
            return applications.ToList();
        }
        #endregion
    }
}

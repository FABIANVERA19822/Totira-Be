using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Queries;
using static Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile.QueryVerifiedProfile;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.VerifiedProfile
{
    public class QueryVerifiedProfileHandler : IQueryHandler<QueryVerifiedProfile, ListTenantVerifiedProfileDto>
    {
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfilesRepository;
        private readonly ILogger<QueryVerifiedProfileHandler> _logger;

        public QueryVerifiedProfileHandler(
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfilesRepository,
            ILogger<QueryVerifiedProfileHandler> logger
            )
        {
            _tenantVerifiedProfilesRepository = tenantVerifiedProfilesRepository;
            _logger = logger;
        }

        public async Task<ListTenantVerifiedProfileDto> HandleAsync(QueryVerifiedProfile query)
        {
            Expression<Func<TenantVerifiedProfile, bool>> expression = (a => (a.Certn == true && a.Jira == true 
                && a.Persona == true && a.IsVerifiedEmailConfirmation == false));

            var info = await _tenantVerifiedProfilesRepository.Get(expression);

            ListTenantVerifiedProfileDto tenantProfileDto = new ListTenantVerifiedProfileDto();
            tenantProfileDto.Count = info.Count();

            if (info.Any())
            {
                var paggingData = info.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

                var data = new List<TenantVerifiedProfileDto>();

                paggingData.ForEach(vp => {
                    data.Add(new TenantVerifiedProfileDto(vp.Id.ToString(),vp.TenantId,vp.Certn,vp.Jira,
                        vp.Persona,vp.IsVerifiedEmailConfirmation,vp.UpdatedOn.Value));
                });

                tenantProfileDto.VerifiedProfiles = SortingProfiles(data, query.SortBy);
            }
            else
            {
                tenantProfileDto.VerifiedProfiles = new List<TenantVerifiedProfileDto>();
                _logger.LogWarning("There are no pending processes of tenant profile validation.");
            }
            return tenantProfileDto;
        }

        #region Helper Methods
        private List<TenantVerifiedProfileDto> SortingProfiles(IEnumerable<TenantVerifiedProfileDto> profiles, EnumVerifiedProfileSortBy? sortBy)
        {
            switch (sortBy)
            {
                case EnumVerifiedProfileSortBy.Id:
                    profiles = profiles.OrderByDescending(s => s.Id);
                    break;
                case EnumVerifiedProfileSortBy.TenantId:
                    profiles = profiles.OrderBy(s => s.TenantId);
                    break;
                case EnumVerifiedProfileSortBy.Certn:
                    profiles = profiles.OrderBy(s => s.Certn);
                    break;
                case EnumVerifiedProfileSortBy.Jira:
                    profiles = profiles.OrderByDescending(s => s.Jira);
                    break;
                case EnumVerifiedProfileSortBy.Persona:
                    profiles = profiles.OrderByDescending(s => s.Persona);
                    break;
                default:
                    profiles = profiles.OrderByDescending(s => s.CreatedOn);
                    break;
            }
            return profiles.ToList();
        }
        #endregion
    }
}

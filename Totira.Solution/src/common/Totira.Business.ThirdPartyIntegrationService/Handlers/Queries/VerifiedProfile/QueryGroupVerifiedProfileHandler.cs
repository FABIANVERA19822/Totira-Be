
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Support.Application.Queries;
using Totira.Business.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using static Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile.QueryGroupVerifiedProfile;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.VerifiedProfile
{
    public class QueryGroupVerifiedProfileHandler : IQueryHandler<QueryGroupVerifiedProfile, ListTenantGroupVerifiedProfileDto>
    {
        private readonly IRepository<TenantGroupVerifiedProfile, Guid> _tenantGroupVerifiedProfilesRepository;
        private readonly ILogger<QueryVerifiedProfileHandler> _logger;

        public QueryGroupVerifiedProfileHandler(
            IRepository<TenantGroupVerifiedProfile, Guid> tenantGroupVerifiedProfilesRepository,
            ILogger<QueryVerifiedProfileHandler> logger
            )
        {
            _tenantGroupVerifiedProfilesRepository = tenantGroupVerifiedProfilesRepository;
            _logger = logger;
        }

        public async Task<ListTenantGroupVerifiedProfileDto> HandleAsync(QueryGroupVerifiedProfile query)
        {
            Expression<Func<TenantGroupVerifiedProfile, bool>> expression = (a => (a.Certn == true && a.Jira == true
                && a.Persona == true && a.IsVerifiedEmailConfirmation == false));

            var info = await _tenantGroupVerifiedProfilesRepository.Get(expression);

            ListTenantGroupVerifiedProfileDto tenantGroupProfileDto = new ListTenantGroupVerifiedProfileDto();
            tenantGroupProfileDto.Count = info.Count();

            if (info.Any())
            {
                var paggingData = info.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

                var data = new List<TenantGroupVerifiedProfileDto>();

                paggingData.ForEach(vp =>
                {
                    data.Add(new TenantGroupVerifiedProfileDto(vp.Id.ToString(), vp.TenantId, vp.Certn, vp.Jira,
                        vp.Persona, vp.IsVerifiedEmailConfirmation, vp.UpdatedOn.Value));
                });

                tenantGroupProfileDto.GroupVerifiedProfiles = SortingProfiles(data, query.SortBy);
            }
            else
            {
                tenantGroupProfileDto.GroupVerifiedProfiles = new List<TenantGroupVerifiedProfileDto>();
                _logger.LogWarning("There are no pending processes of tenant group profile validation.");
            }
            return tenantGroupProfileDto;
        }

        #region Helper Methods
        private List<TenantGroupVerifiedProfileDto> SortingProfiles(IEnumerable<TenantGroupVerifiedProfileDto> profiles, EnumGroupVerifiedProfileSortBy? sortBy)
        {
            switch (sortBy)
            {
                case EnumGroupVerifiedProfileSortBy.Id:
                    profiles = profiles.OrderByDescending(s => s.Id);
                    break;
                case EnumGroupVerifiedProfileSortBy.TenantId:
                    profiles = profiles.OrderBy(s => s.TenantId);
                    break;
                case EnumGroupVerifiedProfileSortBy.Certn:
                    profiles = profiles.OrderBy(s => s.Certn);
                    break;
                case EnumGroupVerifiedProfileSortBy.Jira:
                    profiles = profiles.OrderByDescending(s => s.Jira);
                    break;
                case EnumGroupVerifiedProfileSortBy.Persona:
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

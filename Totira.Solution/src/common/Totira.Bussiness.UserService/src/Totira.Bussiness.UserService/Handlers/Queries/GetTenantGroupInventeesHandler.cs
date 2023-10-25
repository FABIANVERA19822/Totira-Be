
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Bussiness.UserService.Queries.QueryTenantGroupInventees;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantGroupInventeesHandler : IQueryHandler<QueryTenantGroupInventees, ListTenantGroupApplicationProfile>
    {
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationRepository;
        private readonly ILogger<GetTenantGroupInventeesHandler> _logger;

        public GetTenantGroupInventeesHandler(
            IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationRepository,
            ILogger<GetTenantGroupInventeesHandler> logger
            )
        {
            _tenantGroupApplicationRepository = tenantGroupApplicationRepository;
            _logger = logger;
        }

        public async Task<ListTenantGroupApplicationProfile> HandleAsync(QueryTenantGroupInventees query)
        {
            Expression<Func<TenantGroupApplicationProfile, bool>> expression = (g => (g.Email != string.Empty && g.IsVerifiedEmailConfirmation == false));

            var info = await _tenantGroupApplicationRepository.Get(expression);

            ListTenantGroupApplicationProfile tenantProfileDto = new ListTenantGroupApplicationProfile();
            tenantProfileDto.Count = info.Count();

            if (info.Any())
            {
                var paggingData = info.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

                var data = new List<TenantGroupApplicationProfile>();

                paggingData.ForEach(gap => {
                    data.Add(new TenantGroupApplicationProfile(gap.TenantId, gap.FirstName, gap.Email,
                        gap.InvinteeType, gap.Status, gap.IsVerifiedEmailConfirmation, gap.CreatedOn));
                });

                tenantProfileDto.GroupApplicationProfiles = SortingGroupApplications(data, query.SortBy);
            }
            else
            {
                tenantProfileDto.GroupApplicationProfiles = new List<TenantGroupApplicationProfile>();
                _logger.LogWarning("There are no pending processes of tenant group application profile validation.");
            }
            return tenantProfileDto;
        }

        #region Helper Methods
        private List<TenantGroupApplicationProfile> SortingGroupApplications(IEnumerable<TenantGroupApplicationProfile> groupApplications, EnumTenantGroupSortBy? sortBy)
        {
            switch (sortBy)
            {
                case EnumTenantGroupSortBy.TenantId:
                    groupApplications = groupApplications.OrderBy(s => s.TenantId);
                    break;
                case EnumTenantGroupSortBy.FirstName:
                    groupApplications = groupApplications.OrderBy(s => s.FirstName);
                    break;
                case EnumTenantGroupSortBy.Email:
                    groupApplications = groupApplications.OrderByDescending(s => s.Email);
                    break;
                case EnumTenantGroupSortBy.InvinteeType:
                    groupApplications = groupApplications.OrderByDescending(s => s.InvinteeType);
                    break;
                default:
                    groupApplications = groupApplications.OrderByDescending(s => s.CreatedOn);
                    break;
            }
            return groupApplications.ToList();
        }
        #endregion
    }
}

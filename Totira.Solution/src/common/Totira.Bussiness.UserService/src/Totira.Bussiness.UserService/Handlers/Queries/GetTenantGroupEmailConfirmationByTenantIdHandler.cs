
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantGroupEmailConfirmationByTenantIdHandler : IQueryHandler<QueryTenantGroupEmailConfirmationByTenantId, List<TenantGroupApplicationProfile>>
    {
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;
        public GetTenantGroupEmailConfirmationByTenantIdHandler(IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository)
        {
            _tenantGroupApplicationProfileRepository = tenantGroupApplicationProfileRepository;
        }

        public async Task<List<TenantGroupApplicationProfile>> HandleAsync(QueryTenantGroupEmailConfirmationByTenantId query)
        {
            var list = await _tenantGroupApplicationProfileRepository.Get(x => x.TenantId == query.TenantId);
            foreach (var tenantGroupApplicationProfile in list)
            {
                tenantGroupApplicationProfile.IsVerifiedEmailConfirmation = true;
                _tenantGroupApplicationProfileRepository.Update(tenantGroupApplicationProfile);
            }
            return list.ToList();
        }
    }
}

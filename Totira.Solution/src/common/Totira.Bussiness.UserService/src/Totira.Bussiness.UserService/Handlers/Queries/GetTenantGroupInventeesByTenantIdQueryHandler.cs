
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantGroupInventeesByTenantIdQueryHandler: IQueryHandler<QueryTenantGroupInventeesByTenantId, List<TenantGroupApplicationProfile>>
    {
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;
        public GetTenantGroupInventeesByTenantIdQueryHandler(IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository)
        {
            _tenantGroupApplicationProfileRepository=tenantGroupApplicationProfileRepository;
        }

        public async Task<List<TenantGroupApplicationProfile>> HandleAsync(QueryTenantGroupInventeesByTenantId query)
        {
            var list =await  _tenantGroupApplicationProfileRepository.Get(x => x.TenantId == query.TenantId);
            return list.ToList();
        }
    }   
    
}

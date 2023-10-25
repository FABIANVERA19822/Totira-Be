using Totira.Business.TenantService.Domain;
using Totira.Business.TenantService.DTO;
using Totira.Business.TenantService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.TenantService.Handlers.Queries
{
    public class GetTenatnByIdQueryHandler : IQueryHandler<QueryTenantById, GetTenantDto>
    {
        private readonly IRepository<Tenant,Guid> _TenantRepository;
        public GetTenatnByIdQueryHandler(IRepository<Tenant,Guid> tenantRepository)
        {
            _TenantRepository = tenantRepository;
        }
        public async Task<GetTenantDto> HandleAsync(QueryTenantById query)
        {
            var tenant = await _TenantRepository.GetByIdAsync(query.Id);
            var result = new GetTenantDto(tenant.Id, tenant.Name);
            return await Task.FromResult(result);
        }
    }
}

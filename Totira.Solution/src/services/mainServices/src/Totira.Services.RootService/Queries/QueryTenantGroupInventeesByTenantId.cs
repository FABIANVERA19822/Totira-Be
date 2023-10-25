using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryTenantGroupInventeesByTenantId
    {
      
            public QueryTenantGroupInventeesByTenantId(Guid tenantId) => TenantId = tenantId;
            public Guid TenantId { get; set; }
        
    }
}

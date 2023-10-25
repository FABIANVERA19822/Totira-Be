using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantApplicationRoleByTenantId : IQuery
    {

        public QueryTenantApplicationRoleByTenantId(Guid tenantId, Guid applicationDetailsId) { TenantId = tenantId; ApplicationDetailsId = applicationDetailsId; }
        public Guid TenantId { get; set; }
        public Guid ApplicationDetailsId { get; set; }
    }
}

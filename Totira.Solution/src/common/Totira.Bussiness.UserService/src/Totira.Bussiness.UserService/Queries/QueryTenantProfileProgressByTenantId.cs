using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantProfileProgressByTenantId: IQuery
    {
        public QueryTenantProfileProgressByTenantId(Guid tenantId) => TenantId = tenantId;
        public Guid TenantId { get; set; }
    }
}

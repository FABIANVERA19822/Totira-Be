using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryVerificationsProcessStatusById : IQuery
    {
        public Guid TenantId { get; set; }
        public QueryVerificationsProcessStatusById(Guid id) => TenantId = id;
    }
}

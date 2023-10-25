using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTermsAndConditionsByApplicationRequestId : IQuery
    {
        public QueryTermsAndConditionsByApplicationRequestId(Guid applicationRequestId) => ApplicationRequestId = applicationRequestId;

        public Guid ApplicationRequestId { get; }
    }
}

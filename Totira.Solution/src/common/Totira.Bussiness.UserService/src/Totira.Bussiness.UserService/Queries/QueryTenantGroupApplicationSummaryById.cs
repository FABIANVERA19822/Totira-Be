using System;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
	public class QueryTenantGroupApplicationSummaryById : IQuery
    {
        public Guid Id { get; }

        public QueryTenantGroupApplicationSummaryById(Guid id) => Id = id;
    }
}


using System;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
	public class QueryTenantProfileSummaryById : IQuery
    {

        public Guid Id { get; }

        public QueryTenantProfileSummaryById(Guid id) => Id = id;
    }
}


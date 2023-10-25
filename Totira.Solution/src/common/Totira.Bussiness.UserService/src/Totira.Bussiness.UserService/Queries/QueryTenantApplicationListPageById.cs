using System;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
	public class QueryTenantApplicationListPageById : IQuery
    {
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public QueryTenantApplicationListPageById(Guid id , int pageNumber , int pageSize)
        {
            Id = id;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
       
    }
}


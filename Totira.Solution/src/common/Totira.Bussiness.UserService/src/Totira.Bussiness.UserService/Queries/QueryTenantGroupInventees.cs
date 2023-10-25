
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantGroupInventees : IQuery
    {
        public EnumTenantGroupSortBy? SortBy { get; } = EnumTenantGroupSortBy.CreatedOn;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public QueryTenantGroupInventees()
        {

        }
        public QueryTenantGroupInventees(EnumTenantGroupSortBy? sortBy, int pageNumber, int pageSize)
        {
            this.SortBy = sortBy;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public enum EnumTenantGroupSortBy
        {
            TenantId,
            FirstName,
            Email,
            InvinteeType,
            CreatedOn
        }
    }
}

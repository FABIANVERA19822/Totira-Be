
using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile
{
    public class QueryGroupVerifiedProfile : IQuery
    {
        public EnumGroupVerifiedProfileSortBy? SortBy { get; } = EnumGroupVerifiedProfileSortBy.CreatedOn;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public QueryGroupVerifiedProfile()
        {

        }
        public QueryGroupVerifiedProfile(EnumGroupVerifiedProfileSortBy? sortBy, int pageNumber, int pageSize)
        {
            this.SortBy = sortBy;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public enum EnumGroupVerifiedProfileSortBy
        {
            Id,
            TenantId,
            Certn,
            Jira,
            Persona,
            CreatedOn
        }
    }
}

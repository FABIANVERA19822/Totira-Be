using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile
{
    public class QueryVerifiedProfile : IQuery
    {
        public EnumVerifiedProfileSortBy? SortBy { get; } = EnumVerifiedProfileSortBy.CreatedOn;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public QueryVerifiedProfile()
        {

        }
        public QueryVerifiedProfile(EnumVerifiedProfileSortBy? sortBy, int pageNumber, int pageSize)
        {
            this.SortBy = sortBy;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public enum EnumVerifiedProfileSortBy
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

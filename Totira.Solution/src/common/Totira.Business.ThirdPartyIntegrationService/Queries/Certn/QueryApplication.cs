using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.Certn
{
    public class QueryApplication : IQuery
    {
        public EnumApplicantSortBy? SortBy { get; } = EnumApplicantSortBy.CreatedOn;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public QueryApplication()
        {

        }
        public QueryApplication(EnumApplicantSortBy? sortBy, int pageNumber, int pageSize)
        {
            this.SortBy = sortBy;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public enum EnumApplicantSortBy
        {
            Id,
            ApplicantId,
            Status,
            CreatedOn
        }
    }
}

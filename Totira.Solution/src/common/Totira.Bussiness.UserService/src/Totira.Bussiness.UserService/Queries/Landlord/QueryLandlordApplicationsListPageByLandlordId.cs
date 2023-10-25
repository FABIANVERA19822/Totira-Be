using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Landlord
{
    public class QueryLandlordApplicationsListPageByLandlordId : IQuery
    {
        public Guid LandlordId { get; set; }
        public string PropertyId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool Asc { get; set; }
        public SortApplicationsBy SortBy { get; set; }
        public QueryLandlordApplicationsListPageByLandlordId(Guid landlordId, string propertyId, SortApplicationsBy sortBy,bool asc, int pageNumber, int pageSize)
        {
            LandlordId = landlordId;
            PropertyId = propertyId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortBy = sortBy;
            Asc= asc;
        }
    }
}

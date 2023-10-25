
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Landlord
{
    public class QueryPropertyClaimsDisplayByLandlordId: IQuery
    {
        public QueryPropertyClaimsDisplayByLandlordId(Guid landlordId, int pageNumber, int pageSize)
        {
            LandlordId = landlordId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        [Required]
        public Guid LandlordId { get; set; }
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }

    }
}

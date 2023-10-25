
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Landlord
{
    public class QueryPropertyClaimsDisplayByLandlordId: IQuery
    {
        public QueryPropertyClaimsDisplayByLandlordId(Guid landlordId, 
                                                      int pageNumber, 
                                                      int pageSize,
                                                      bool orderBy)
        {
            LandlordId = landlordId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Descending = orderBy;
        }

        [Required]
        public Guid LandlordId { get; set; }
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }
        public bool Descending { get; set; } = true;        
    }

}

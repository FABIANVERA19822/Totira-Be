using System.ComponentModel.DataAnnotations;
using Totira.Bussiness.UserService.Enums;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Landlord
{
    public record QueryLandlordPropertiesDisplayByLandlordId : IQuery
    {
        public QueryLandlordPropertiesDisplayByLandlordId(
            Guid landlordId,
            int pageNumber,
            int pageSize,
            LandlordPropertyDisplaySortByEnum? orderBy,
            bool descending)
        {
            LandlordId = landlordId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
            Descending = descending;
        }

        [Required]
        public Guid LandlordId { get; set; }
        [Required]
        public int PageNumber { get; set; } = 1;
        [Required]
        public int PageSize { get; set; } = 5;
        public LandlordPropertyDisplaySortByEnum? OrderBy { get; set; } = LandlordPropertyDisplaySortByEnum.ApplicationCount;
        public bool Descending { get; set; } = true;
    }
}

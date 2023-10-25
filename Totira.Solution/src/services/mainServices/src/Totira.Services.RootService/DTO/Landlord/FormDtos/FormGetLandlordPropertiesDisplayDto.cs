using Totira.Services.RootService.Enums;

namespace Totira.Services.RootService.DTO.Landlord.FormDtos
{
    public class FormGetLandlordPropertiesDisplayDto
    {
        public Guid LandlordId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public LandlordPropertyDisplaySortByEnum? OrderBy { get; set; } = LandlordPropertyDisplaySortByEnum.ApplicationCount;
        public bool Descending { get; set; } = true;
    }
}

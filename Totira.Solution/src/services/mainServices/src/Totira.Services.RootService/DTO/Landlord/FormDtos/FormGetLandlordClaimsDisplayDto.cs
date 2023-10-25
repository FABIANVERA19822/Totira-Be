namespace Totira.Services.RootService.DTO.Landlord.FormDtos
{
    public class FormGetLandlordClaimsDisplayDto
    {
        public Guid LandlordId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool Descending { get; set; } = true;
    }
}

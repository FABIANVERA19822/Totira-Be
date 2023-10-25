namespace Totira.Bussiness.UserService.DTO.Landlord
{
    public class PropertyDisplayDto
    {
        public Guid PropertyDisplayId { get; set; }
        public string ML_Num { get; set; }
        public string Location { get; set; }
        public string Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public long Price { get; set; }
        public string ListingDate { get; set; }
        public long ApplicationCount { get; set; } = 0;
    }
}

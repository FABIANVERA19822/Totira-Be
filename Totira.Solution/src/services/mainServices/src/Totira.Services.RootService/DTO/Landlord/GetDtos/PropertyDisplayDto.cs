namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class PropertyDisplayDto
    {
        public Guid PropertyDisplayId { get; set; }
        public string ML_Num { get; set; }
        public string Location { get; set; }
        public int Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public long Price { get; set; }
        public string AvailableDate { get; set; }
        public int ApplicationCount { get; set; } = 0;
    }
}

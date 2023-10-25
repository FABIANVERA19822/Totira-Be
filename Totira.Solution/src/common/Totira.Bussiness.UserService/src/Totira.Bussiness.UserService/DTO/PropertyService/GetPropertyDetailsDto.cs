namespace Totira.Bussiness.UserService.DTO.PropertyService
{
    public class GetPropertyDetailsDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime? ListingEntryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string FrontingOnNSEW { get; set; } = string.Empty;
        public string ApproxSquareFootage { get; set; } = string.Empty;
        public Decimal LotDepth { get; set; }
        public Decimal LotFront { get; set; }
        public Decimal Bedrooms { get; set; }
        public Decimal Washrooms { get; set; }
        public Decimal ParkingSpaces { get; set; }
        public Decimal TotalParkingSpaces { get; set; }
        public Decimal GarageSpaces { get; set; }
        public string PetsPermitted { get; set; } = string.Empty;  //condo
        public string RemarksForClients { get; set; } = string.Empty;
        public string AirConditioning { get; set; } = string.Empty;
        public string Balcony { get; set; } = string.Empty;       //condo
        public string CableTVIncluded { get; set; } = string.Empty;
        public string Furnished { get; set; } = string.Empty;
        public string HeatIncluded { get; set; } = string.Empty;
        public string HydroIncluded { get; set; } = string.Empty;
        public Decimal KitchensPlus { get; set; }
        public string LaundryAccess { get; set; } = string.Empty;
        public string ParkingIncluded { get; set; } = string.Empty;
        public string PrivateEntrance { get; set; } = string.Empty;
        public string Pool { get; set; } = string.Empty;

        public List<string> BuildingAmenities { get; set; } = new List<string>(); //condo
        public List<string> PropertyFeatures { get; set; } = new List<string>();

        public Decimal OriginalPrice { get; set; }
        public Decimal ListPrice { get; set; }
    }
}

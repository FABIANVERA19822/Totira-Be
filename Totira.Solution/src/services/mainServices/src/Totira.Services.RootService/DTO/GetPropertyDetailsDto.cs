namespace Totira.Services.RootService.DTO
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
        public decimal LotDepth { get; set; }
        public decimal LotFront { get; set; }
        public decimal Bedrooms { get; set; }
        public decimal Washrooms { get; set; }
        public decimal ParkingSpaces { get; set; }
        public decimal TotalParkingSpaces { get; set; }
        public decimal GarageSpaces { get; set; }
        public string PetsPermitted { get; set; } = string.Empty;  //condo
        public string RemarksForClients { get; set; } = string.Empty;
        public string AirConditioning { get; set; } = string.Empty;
        public string Balcony { get; set; } = string.Empty;       //condo
        public string CableTVIncluded { get; set; } = string.Empty;
        public string Furnished { get; set; } = string.Empty;
        public string HeatIncluded { get; set; } = string.Empty;
        public string HydroIncluded { get; set; } = string.Empty;
        public decimal KitchensPlus { get; set; }
        public string LaundryAccess { get; set; } = string.Empty;
        public string ParkingIncluded { get; set; } = string.Empty;
        public string PrivateEntrance { get; set; } = string.Empty;
        public string Pool { get; set; } = string.Empty;
        public List<string> BuildingAmenities { get; set; } = new List<string>(); //condo
        public List<string> PropertyFeatures { get; set; } = new List<string>();
        public decimal OriginalPrice { get; set; }
        public decimal ListPrice { get; set; }


        public GetPropertyDetailsDto(string id, string area, string address, string frontingOnNSEW, string approxSquareFootage,
          decimal lotDepth, decimal lotFront, decimal bedrooms, decimal washrooms, decimal parkingSpaces, decimal totalParkingSpaces,
          decimal garageSpaces, string petsPermitted, string remarksForClients, string airConditioning, string balcony, string cableTVIncluded,
          string furnished, string heatIncluded, string hydroIncluded, decimal kitchensPlus, string laundryAccess, string parkingIncluded,
           string privateEntrance, string pool, List<string> buildingAmenities, List<string> propertyFeatures, decimal originalPrice, decimal listPrice)
        {
            Id = id;
            Area = area;
            Address = address;
            FrontingOnNSEW = frontingOnNSEW;
            ApproxSquareFootage = approxSquareFootage;
            LotDepth = lotDepth;
            LotFront = lotFront;
            Bedrooms = bedrooms;
            Washrooms = washrooms;
            ParkingSpaces = parkingSpaces;
            TotalParkingSpaces = totalParkingSpaces;
            GarageSpaces = garageSpaces;
            PetsPermitted = petsPermitted;
            RemarksForClients = remarksForClients;
            AirConditioning = airConditioning;
            Balcony = balcony;
            CableTVIncluded = cableTVIncluded;
            Furnished = furnished;
            HeatIncluded = heatIncluded;
            HydroIncluded = hydroIncluded;
            KitchensPlus = kitchensPlus;
            LaundryAccess = laundryAccess;
            ParkingIncluded = parkingIncluded;
            PrivateEntrance = privateEntrance;
            Pool = pool;
            BuildingAmenities = buildingAmenities;
            PropertyFeatures = propertyFeatures;
            OriginalPrice = originalPrice;
            ListPrice = listPrice;

        }

        public GetPropertyDetailsDto() { }


 
    }
}

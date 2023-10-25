namespace Totira.Bussiness.PropertiesService.DTO
{
    public class GetPropertyDetailsDto
    {
        public string Id { get; set; } = string.Empty;

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


        public GetPropertyDetailsDto(string id, string area, string address, string frontingOnNSEW, string approxSquareFootage,
          Decimal lotDepth, Decimal lotFront, Decimal bedrooms, Decimal washrooms, Decimal parkingSpaces, Decimal totalParkingSpaces,
          Decimal garageSpaces, string petsPermitted, string remarksForClients, string airConditioning, string balcony, string cableTVIncluded,
          string furnished, string heatIncluded, string hydroIncluded, Decimal kitchensPlus, string laundryAccess, string parkingIncluded,
           string privateEntrance, string pool, List<string> buildingAmenities , List<string> propertyFeatures, Decimal originalPrice, Decimal listPrice)
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
            BuildingAmenities= buildingAmenities;
            PropertyFeatures= propertyFeatures;
            OriginalPrice = originalPrice;
            ListPrice = listPrice;

        }
        public GetPropertyDetailsDto() { }
        //mock
        /* public GetPropertyDetailsDto()
         {
             Ml_num = "C536651";
             Bedrooms = 2;
             Province = "Ontario";
             ListPrice = 12000;

         }*/
    }
}

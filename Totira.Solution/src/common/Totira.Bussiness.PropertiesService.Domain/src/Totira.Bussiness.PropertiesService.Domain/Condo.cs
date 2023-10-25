using MongoDB.Bson.Serialization.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Totira.Bussiness.PropertiesService.Domain
{
    [BsonIgnoreExtraElements]
    public class Condo
    {
        public Condo() { }
        public string EnsuiteLaundry { get; set; } = string.Empty;
        public List<string> ParkingSpot { get; set; } = new List<string>();
        public string SecurityGuardSystem { get; set; } = string.Empty;
        public List<string> BuildingAmenities { get; set; } = new List<string>();
        public string CondoRegistryOffice { get; set; } = string.Empty;
        public decimal CondoCorp { get; set; }
        public decimal GarageParkSpaces { get; set; }
        public List<string> ParkingType { get; set; } = new List<string>();
        public string ParkingDrive { get; set; } = string.Empty; 
        public string PetsPermitted { get; set; } = string.Empty;
        public string SharesPer { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Locker { get; set; } = string.Empty;
        public string LockerNum { get; set; } = string.Empty;
        public decimal Maintenance { get; set; }
        public string LockerLevel { get; set; } = string.Empty;
        public string LockerUnit { get; set; } = string.Empty;
        public string Exposure { get; set; } = string.Empty;
        public string BuildingInsuranceIncluded { get; set; } = string.Empty;
        public List<string> ParkingLegalDescription { get; set; }
        public string Balcony { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string CondoTaxesIncluded { get; set; } = string.Empty;

    }
}
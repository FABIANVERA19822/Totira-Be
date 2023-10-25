using MongoDB.Bson.Serialization.Attributes;

namespace Totira.Bussiness.PropertiesService.Domain
{
    [BsonIgnoreExtraElements]
    public class Residential
    {
       
        public string UtilitiesHydro { get; set; } = string.Empty;
        public string UtilitiesGas { get; set; } = string.Empty;
        public string Pool { get; set; } = string.Empty;
        public string Waterfront { get; set; } = string.Empty;
        public string LeaseAgreement { get; set; } = string.Empty; 
        public string FrontingOnNSEW { get; set; } = string.Empty;
        public string Drive { get; set; } = string.Empty;
        public string FarmAgriculture { get; set; } = string.Empty;
        public decimal GarageSpaces { get; set; }
        public List<string> OtherStructures { get; set; } = new List<string>();
        public string PaymentFrequency { get; set; } = string.Empty;
        public string UtilitiesCable { get; set; } = string.Empty;
        public string Water { get; set; } = string.Empty;
        public string WaterSupplyTypes { get; set; } = string.Empty;
        public string LotIrregularities { get; set; } = string.Empty;
        public string LeaseTerm { get; set; } = string.Empty;
        public string LotFrontIncomplete { get; set; } = string.Empty;
        public string LeasedTerms { get; set; } = string.Empty;
        public string ParcelOfTiedLand { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Link_Comment { get; set; } = string.Empty;
        public string LotSizeCode { get; set; } = string.Empty;
        public decimal LotDepth { get; set; }
        public decimal LotFront { get; set; }
        public string UtilitiesTelephone { get; set; } = string.Empty;
        public string Acreage { get; set; } = string.Empty;
        public string LegalDescription { get; set; } = string.Empty;
        public string Sewers { get; set; } = string.Empty;



        public Residential() { }
    }

}
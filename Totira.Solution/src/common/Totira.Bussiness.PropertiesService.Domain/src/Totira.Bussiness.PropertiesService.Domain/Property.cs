using Totira.Support.Persistance; 
using Totira.Support.Persistance.Document;
using MongoDB.Bson.Serialization.Attributes;

namespace Totira.Bussiness.PropertiesService.Domain
{
    [BsonIgnoreExtraElements]
    public class Property : DocumentMLS, IAuditable, IEquatable<Property>
    {
        public string PropertyType { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public decimal DaysOnMarket { get; set; }
        public string Extras { get; set; } = string.Empty;
        public string GarageType { get; set; } = string.Empty;
        public DateTime? ListingEntryDate { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string DirectionsCrossStreets { get; set; } = string.Empty;
        public string DisplayAddressOnInternet { get; set; } = string.Empty;
        public DateTime? SuspendedDate { get; set; }
        public string Elevator { get; set; } = string.Empty;
        public string FireplaceStove { get; set; } = string.Empty;
        public string Furnished { get; set; } = string.Empty;
        public string HeatIncluded { get; set; } = string.Empty;
        public string HydroIncluded { get; set; } = string.Empty;
        public string DistributeToInternetPortals { get; set; } = string.Empty;
        public string OutofAreaMunicipality { get; set; } = string.Empty;
        public decimal ParkCostMo { get; set; }
        public decimal PerListingPrice { get; set; }
        public string PriorLSC { get; set; } = string.Empty;
        public string ParkingIncluded { get; set; } = string.Empty;
        public string ParcelId { get; set; } = string.Empty;
        public string FamilyRoom { get; set; } = string.Empty;
        public DateTime? TerminatedDate { get; set; }
        public string HeatSource { get; set; } = string.Empty;
        public string HeatType { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal ParkingSpaces { get; set; }
        public decimal Rooms { get; set; }
        public decimal RoomsPlus { get; set; }
        public string Uffi { get; set; } = string.Empty;
        public DateTime? UnavailableDate { get; set; }
        public string SellerPropertyInfoStatement { get; set; } = string.Empty;
        public DateTime? VirtualTourUploadDate { get; set; }
        public string WaterIncluded { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtensionEntryDate { get; set; }
        public decimal TaxYear { get; set; }
        public string ApproxAge { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Zoning { get; set; } = string.Empty;
        public DateTime? TimestampSql { get; set; }
        public string MunicipalityCode { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Community { get; set; } = string.Empty;
        public string CertLevel { get; set; } = string.Empty;
        public string EnergyCertification { get; set; } = string.Empty;
        public string PhyHandiEquipped { get; set; } = string.Empty;
        public string AirConditioning { get; set; } = string.Empty;
        public string PortionPropertyLeaseSrch { get; set; } = string.Empty;
        public string PortionLeaseComments { get; set; } = string.Empty;
        public string Assignment { get; set; } = string.Empty;
        public string FractionalOwnership { get; set; } = string.Empty;
        public string RemarksForClients { get; set; } = string.Empty;
        public decimal AddlMonthlyFees { get; set; }
        public string Address { get; set; } = string.Empty;
        public string AllInclusive { get; set; } = string.Empty;
        public string AptUnit { get; set; } = string.Empty;
        public decimal AssessmentYear { get; set; }
        public decimal Washrooms { get; set; }
        public decimal Bedrooms { get; set; }
        public decimal BedroomsPlus { get; set; }
        public string CableTVIncluded { get; set; } = string.Empty;
        public string CacIncluded { get; set; } = string.Empty;
        public DateTime? SoldDate { get; set; }
        public string CentralVac { get; set; } = string.Empty;
        public DateTime? ConditionalExpiryDate { get; set; }
        public string CommissionCoOpBrokerage { get; set; } = string.Empty;
        public string CommonElementsIncluded { get; set; } = string.Empty;
        public decimal KitchensPlus { get; set; }
        public string LaundryAccess { get; set; } = string.Empty;
        public string LaundryLevel { get; set; } = string.Empty;
        public DateTime? ContractDate { get; set; }
        public decimal ListPrice { get; set; }
        public string LastStatus { get; set; } = string.Empty;
        public decimal MapColumn { get; set; }
        public decimal Map { get; set; }
        public string MapRow { get; set; } = string.Empty;
        public decimal Kitchens { get; set; }
        public string PossessionRemarks { get; set; } = string.Empty;
        public string PropertyMgmtCo { get; set; } = string.Empty;
        public string PrivateEntrance { get; set; } = string.Empty;
        public string Retirement { get; set; } = string.Empty;
        public string ListBrokerage { get; set; } = string.Empty;
        public string SaleLease { get; set; } = string.Empty;
        public decimal SoldPrice { get; set; }
        public string ApproxSquareFootage { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string StreetDirection { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string StreetAbbreviation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public decimal Taxes { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string VirtualTourURL { get; set; } = string.Empty;
        public string CommunityCode { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
        public decimal Assessment { get; set; }
        public string TypeOwnSrch { get; set; } = string.Empty;
        public string TypeOwn1Out { get; set; } = string.Empty;
        public string MunicipalityDistrict { get; set; } = string.Empty;
        public string Municipality { get; set; } = string.Empty;
        public DateTime? PixUpdtedDt { get; set; }
        public DateTime? OpenHouseUpDtTimestamp { get; set; }
        public string GreenPropInfoStatement { get; set; } = string.Empty;
        public DateTime? PossessionDate { get; set; }
        public string WaterBodyName { get; set; } = string.Empty;
        public string WaterBodyType { get; set; } = string.Empty;
        public decimal WaterFrontage { get; set; }
        public string ShorelineAllowance { get; set; } = string.Empty;
        public string ShorelineExposure { get; set; } = string.Empty;
        public decimal TotalParkingSpaces { get; set; }
        
        public List<string> PropertyFeatures { get; set; } = new List<string>();
        public List<string> Exterior { get; set; } = new List<string>();
        public List<decimal> WashroomsTypePcs { get; set; } = new List<decimal>();
        public List<decimal> WashroomsType { get; set; } = new List<decimal>();
        public List<string> WashroomsTypeLevel { get; set; } = new List<string>();
        public List<string> OpenHouseTo { get; set; } = new List<string>();
        public List<string> OpenHouseFrom { get; set; } = new List<string>();
        public List<DateTime?> OpenHouseDate { get; set; } = new List<DateTime?>();
        public List<string> Open_House_Link { get; set; } = new List<string>();
        public List<string> Open_House_Type { get; set; } = new List<string>();
        public List<string> AccessToProperty { get; set; } = new List<string>();
        public List<string> PortionPropertyLease { get; set; } = new List<string>();
        public List<string> Basement { get; set; } = new List<string>();
        public List<string> Level { get; set; } = new List<string>();
        public List<string> SpecialDesignation { get; set; } = new List<string>();
        public List<string> WaterFeatures { get; set; } = new List<string>();
        public List<string> Shoreline { get; set; } = new List<string>();
        public List<string> AlternativePower { get; set; } = new List<string>();
        public List<string> EasementsRestrictions { get; set; } = new List<string>();
        public List<string> RuralServices { get; set; } = new List<string>();
        public List<string> WaterfrontAccBldgs { get; set; } = new List<string>();
        public List<string> WaterDeliveryFeatures { get; set; } = new List<string>();
        public List<string> Sewage { get; set; } = new List<string>();


        //Rooms
        public string Room1 { get; set; } = string.Empty;
        public string Room2 { get; set; } = string.Empty;
        public string Room3 { get; set; } = string.Empty;
        public string Room4 { get; set; } = string.Empty;
        public string Room5 { get; set; } = string.Empty;
        public string Room6 { get; set; } = string.Empty;
        public string Room7 { get; set; } = string.Empty;
        public string Room8 { get; set; } = string.Empty;
        public string Room9 { get; set; } = string.Empty;
        public string Room10 { get; set; } = string.Empty;
        public string Room11 { get; set; } = string.Empty;
        public string Room12 { get; set; } = string.Empty;


        //RoomsDescriptions
        public string Room1Desc1 { get; set; } = string.Empty;
        public string Room1Desc2 { get; set; } = string.Empty;
        public string Room1Desc3 { get; set; } = string.Empty;

        public string Room2Desc1 { get; set; } = string.Empty;
        public string Room2Desc2 { get; set; } = string.Empty;
        public string Room2Desc3 { get; set; } = string.Empty;

        public string Room3Desc1 { get; set; } = string.Empty;
        public string Room3Desc2 { get; set; } = string.Empty;
        public string Room3Desc3 { get; set; } = string.Empty;

        public string Room4Desc1 { get; set; } = string.Empty;
        public string Room4Desc2 { get; set; } = string.Empty;
        public string Room4Desc3 { get; set; } = string.Empty;

        public string Room5Desc1 { get; set; } = string.Empty;
        public string Room5Desc2 { get; set; } = string.Empty;
        public string Room5Desc3 { get; set; } = string.Empty;

        public string Room6Desc1 { get; set; } = string.Empty;
        public string Room6Desc2 { get; set; } = string.Empty;
        public string Room6Desc3 { get; set; } = string.Empty;

        public string Room7Desc1 { get; set; } = string.Empty;
        public string Room7Desc2 { get; set; } = string.Empty;
        public string Room7Desc3 { get; set; } = string.Empty;

        public string Room8Desc1 { get; set; } = string.Empty;
        public string Room8Desc2 { get; set; } = string.Empty;
        public string Room8Desc3 { get; set; } = string.Empty;

        public string Room9Desc1 { get; set; } = string.Empty;
        public string Room9Desc2 { get; set; } = string.Empty;
        public string Room9Desc3 { get; set; } = string.Empty;

        public string Room10Desc1 { get; set; } = string.Empty;
        public string Room10Desc2 { get; set; } = string.Empty;
        public string Room10Desc3 { get; set; } = string.Empty;

        public string Room11Desc1 { get; set; } = string.Empty;
        public string Room11Desc2 { get; set; } = string.Empty;
        public string Room11Desc3 { get; set; } = string.Empty;

        public string Room12Desc1 { get; set; } = string.Empty;
        public string Room12Desc2 { get; set; } = string.Empty;
        public string Room12Desc3 { get; set; } = string.Empty;

        //RoomsArea

        public string Room1Length { get; set; } = string.Empty;
        public string Room2Length { get; set; } = string.Empty;
        public string Room3Length { get; set; } = string.Empty;
        public string Room4Length { get; set; } = string.Empty;
        public string Room5Length { get; set; } = string.Empty;
        public string Room6Length { get; set; } = string.Empty;
        public string Room7Length { get; set; } = string.Empty;
        public string Room8Length { get; set; } = string.Empty;
        public string Room9Length { get; set; } = string.Empty;
        public string Room10Length { get; set; } = string.Empty;
        public string Room11Length { get; set; } = string.Empty;
        public string Room12Length { get; set; } = string.Empty;

        public string Room1Width { get; set; } = string.Empty;
        public string Room2Width { get; set; } = string.Empty;
        public string Room3Width { get; set; } = string.Empty;
        public string Room4Width { get; set; } = string.Empty;
        public string Room5Width { get; set; } = string.Empty;
        public string Room6Width { get; set; } = string.Empty;
        public string Room7Width { get; set; } = string.Empty;
        public string Room8Width { get; set; } = string.Empty;
        public string Room9Width { get; set; } = string.Empty;
        public string Room10Width { get; set; } = string.Empty;
        public string Room11Width { get; set; } = string.Empty;
        public string Room12Width { get; set; } = string.Empty;



        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public Residential  residential { get; set; } = new ();
        public Condo condo { get; set; } = new ();

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy  { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(Property? other)
        {
            throw new NotImplementedException();
        }
    }
}
namespace Totira.Bussiness.UserService.DTO;

public class GetTenantInformationForCertnApplicationDto
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Birthday { get; set; } = default!;
    public string SocialInsuranceNumber { get; set; } = default!;
    public List<AddressDto> Addresses { get; set; } = new();
    public PropertyDto PropertyLocation { get; set; } = new();

    public class AddressDto
    {
        public string Address { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public bool Current { get; set; }
    }

    public class PropertyDto
    {
        public PropertyDto()
        {
            Address = "1410 Blanshard St";
            City = "Victoria";
            ProvinceState = "BC";
            County = string.Empty;
            Country = "CA";
            PostalCode = "V8W2J2";
            LocationType = "Property Location";
        }

        public string Address { get; set; }
        public string City { get; set; }
        public string ProvinceState { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string LocationType { get; set; }
    }
}

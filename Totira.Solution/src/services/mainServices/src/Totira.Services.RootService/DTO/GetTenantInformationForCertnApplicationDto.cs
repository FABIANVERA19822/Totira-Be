namespace Totira.Services.RootService.DTO;

public class GetTenantInformationForCertnApplicationDto
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Birthday { get; set; } = default!;
    public string SocialInsuranceNumber { get; set; } = default!;
    public List<AddressDto> Addresses { get; set; } = new();
    public PropertyDto PropertyLocation { get; set; } = new();

    public string ErrorMessage { get; set; }

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
        public string Address { get; set; } = default!;
        public string City { get; set; } = default!;
        public string ProvinceState { get; set; } = default!;
        public string County { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string LocationType { get; set; } = default!;
    }
}

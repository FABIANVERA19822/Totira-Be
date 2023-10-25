using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateTenantRentalHistoriesCommand")]
    public class CreateTenantRentalHistoriesCommand : ICommand
    {

        public Guid TenantId { get; set; }
        public CustomDate? RentalStartDate { get; set; }
        public bool CurrentlyLivingHere { get; set; }
        public CustomDate? RentalEndDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public bool IsFeedbackRequest { get; set; }
        public LandlordContactInformation? ContactInformation { get; set; } = new LandlordContactInformation();
    }
    public class LandlordContactInformation
    {
        public string Relationship { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public RentalHistoriesPhoneNumber PhoneNumber { get; set; } = new RentalHistoriesPhoneNumber();
        public string EmailAddress { get; set; } = string.Empty;
    }
    public class CustomDate
    {
        public CustomDate(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }
    public class RentalHistoriesPhoneNumber
    {

        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}


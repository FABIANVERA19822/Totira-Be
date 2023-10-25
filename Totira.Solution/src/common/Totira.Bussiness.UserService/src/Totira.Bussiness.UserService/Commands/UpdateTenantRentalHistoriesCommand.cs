using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("UpdateTenantRentalHistoriesCommand")]
    public class UpdateTenantRentalHistoriesCommand : ICommand
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

}


using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{

    [RoutingKey("UpdateTenantApplicationDetailsCommand")]
    public class UpdateTenantApplicationDetailsCommand : ICommand
    {
        public Guid Id { get; set; }
        public ApplicationDetailEstimatedMove? EstimatedMove { get; set; }
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants();
        public bool Smoker { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public List<ApplicationDetailCar>? Cars { get; set; }
        public bool? IsVerificationsRequested { get; set; }
    }

}

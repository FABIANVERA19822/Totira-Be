using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("UpdateTenantApplicationDetailsCommand")]
    public class UpdateTenantApplicationDetailsCommand : ICommand
    {
        public Guid Id { get; set; }
        public ApplicationDetailEstimatedMove EstimatedMove { get; set; } = new ApplicationDetailEstimatedMove(0, 0);
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants(0, 0);
        public bool Smoker { get; set; }
        public int PetNumber { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public List<ApplicationDetailCar>? Cars { get; set; }
        public bool? IsVerificationsRequested { get; set; }
    }
}

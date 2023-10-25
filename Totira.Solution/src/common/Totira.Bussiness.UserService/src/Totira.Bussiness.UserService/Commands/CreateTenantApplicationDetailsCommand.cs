using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantApplicationDetailsCommand")]
    public class CreateTenantApplicationDetailsCommand : ICommand
    {
        public Guid Id { get; set; }
        public ApplicationDetailEstimatedMove? EstimatedMove { get; set; }
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants(0, 0);
        public bool Smoker { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public List<ApplicationDetailCar>? Cars { get; set; }
    }

    public class ApplicationDetailCar
    {
        public string Model { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }

    }

    public class ApplicationDetailPet
    {
        public string Type { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ApplicationDetailOccupants
    {
        public ApplicationDetailOccupants(int adults, int children)
        {
            Adults = adults;
            Children = children;
        }
        public int Children { get; set; }
        public int Adults { get; set; }
    }

    public class ApplicationDetailEstimatedMove
    {
        public ApplicationDetailEstimatedMove(int month, int year)
        {
            Month = month;
            Year = year;
        }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}

using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantApplicationDetails : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid TenantId { get; set; }
        public ApplicationDetailEstimatedMove? EstimatedMove { get; set; }
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants(0, 0);
        public bool Smoker { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public List<ApplicationDetailCar>? Cars { get; set; }
        public bool ?IsVerificationsRequested { get; set; } = false;
        public bool ?IsProfileValidationComplete { get; set; } = false;
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicationDetailCar
    {
        public ApplicationDetailCar(string plate, int year, string make, string model)
        {
            Plate = plate;
            Year = year;
            Make = make;
            Model = model;
        }

        public string Model { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public int Year { get; set; }

    }

    public class ApplicationDetailPet
    {
        public ApplicationDetailPet(string type, string description, string size)
        {
            Type = type;
            Description = description;
            Size = size;
        }

        public string Type { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ApplicationDetailOccupants
    {
        public ApplicationDetailOccupants(int adults, int childrens)
        {
            Adults = adults;
            Childrens = childrens;
        }

        public int Childrens { get; set; }
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

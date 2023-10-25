namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantApplicationDetailsDto
    {
        public Guid Id { get; set; }
        public ApplicationDetailEstimatedMove? EstimatedMove { get; set; }
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants(0, 0);
        public bool? Pet { get; set; }
        public bool? Car { get; set; }
        public bool? Smoker { get; set; }
        public int PetsNumber { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public int CarsNumber { get; set; }
        public bool IsVerificationsRequested { get; set; } = false;
        public List<ApplicationDetailCar>? Cars { get; set; }

        public bool IsProfileValidationComplete { get; set; } = false;
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
        public ApplicationDetailPet(string size, string description, string type)
        {
            Size = size;
            Description = description;
            Type = type;
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
            Children = childrens;
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

namespace Totira.Services.RootService.DTO
{
    public class GetTenantApplicationDetailsDto
    {
        public Guid Id { get; set; }
        public ApplicationDetailEstimatedMove EstimatedMove { get; set; } = new ApplicationDetailEstimatedMove();
        public string EstimatedRent { get; set; } = string.Empty;
        public ApplicationDetailOccupants Occupants { get; set; } = new ApplicationDetailOccupants();
        public bool? Pet { get; set; }
        public bool? Car { get; set; }
        public bool? Smoker { get; set; }
        public int PetsNumber { get; set; }
        public List<ApplicationDetailPet>? Pets { get; set; }
        public bool IsVerificationsRequested { get; set; } = false;
        public bool IsProfileValidationComplete { get; set; } = false;
        public int CarsNumber { get; set; }
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
        public int Children { get; set; }
        public int Adults { get; set; }
    }

    public class ApplicationDetailEstimatedMove
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }


}

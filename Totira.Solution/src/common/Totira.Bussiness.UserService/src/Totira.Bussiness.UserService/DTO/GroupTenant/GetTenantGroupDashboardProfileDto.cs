namespace Totira.Bussiness.UserService.DTO.GroupTenant;

public class GetTenantGroupDashboardProfileDto
{
    public GetTenantGroupDashboardProfileDto(Guid mainTenantId, Guid applicationRequestId)
    {
        MainTenantId = mainTenantId;
        ApplicationRequestId = applicationRequestId;
    }

    public Guid ApplicationRequestId { get; set; }
    public Guid MainTenantId { get; set; }
    public ApplicationDetailsDto? ApplicationDetails { get; set; }
    public List<TenantDetailDto> Tenants { get; set; } = new();
}

public class ApplicationDetailsDto
{
    public ApplicationDetailsDto(
        double totalIncomes,
        ApplicationDetailsDateDto? estimatedMove,
        string estimatedRent,
        ApplicationDetailsOccupantsDto? occupants,
        bool pet,
        List<ApplicationDetailsPetDto>? pets,
        bool car,
        List<ApplicationDetailsCarDto>? cars)
    {
        TotalIncomes = totalIncomes;
        EstimatedMove = estimatedMove;
        EstimatedRent = estimatedRent;
        Occupants = occupants;
        Pet = pet;
        Pets = pets;
        Car = car;
        Cars = cars;
    }

    public double TotalIncomes { get; set; }
    public ApplicationDetailsDateDto? EstimatedMove { get; set; }
    public string EstimatedRent { get; set; } = default!;
    public ApplicationDetailsOccupantsDto? Occupants { get; set; }
    public bool Pet { get; set; }
    public List<ApplicationDetailsPetDto>? Pets { get; set; }
    public bool Car { get; set; }
    public List<ApplicationDetailsCarDto>? Cars { get; set; }
}
public class ApplicationDetailsDateDto
{
    public ApplicationDetailsDateDto(int month, int year)
    {
        Month = month;
        Year = year;
    }

    public int Month { get; set; }
    public int Year { get; set; }
}

public class ApplicationDetailsOccupantsDto
{
    public ApplicationDetailsOccupantsDto(int adults, int children)
    {
        Adults = adults;
        Children = children;
    }

    public int Adults { get; set; }
    public int Children { get; set; }
}

public class ApplicationDetailsPetDto
{
    public ApplicationDetailsPetDto(string type, string size, string description)
    {
        Type = type;
        Size = size;
        Description = description;
    }

    public string Type { get; set; } = default!;
    public string Size { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public class ApplicationDetailsCarDto
{
    public ApplicationDetailsCarDto(string model, string make, string plate, int year)
    {
        Model = model;
        Make = make;
        Plate = plate;
        Year = year;
    }

    public string Model { get; set; } = default!;
    public string Make { get; set; } = default!;
    public string Plate { get; set; } = default!;
    public int Year { get; set; }
}

public class TenantDetailDto
{
    public TenantDetailDto(GetTenantProfileImageDto? profileImage, bool isMain, bool isGuarantor, GetTenantVerifiedbyProfileDto? detail, string name)
    {
        ProfileImage = profileImage;
        IsMain = isMain;
        IsGuarantor = isGuarantor;
        Detail = detail;
        Name = name;
    }

    public string Name { get; set; }
    public GetTenantProfileImageDto? ProfileImage { get; set; }
    public bool IsMain { get; set; }
    public bool IsGuarantor { get; set; }
    public GetTenantVerifiedbyProfileDto? Detail { get; set; }
}
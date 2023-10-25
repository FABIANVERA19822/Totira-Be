namespace Totira.Services.RootService.DTO.GroupTenant;

public class GetTenantGroupDashboardProfileDto
{
    public Guid ApplicationRequestId { get; set; }
    public Guid MainTenantId { get; set; }
    public ApplicationDetailsDto? ApplicationDetails { get; set; }
    public List<TenantDetailDto> Tenants { get; set; } = new();
}

public class ApplicationDetailsDto
{
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
    public int Month { get; set; }
    public int Year { get; set; }
}

public class ApplicationDetailsOccupantsDto
{
    public int Adults { get; set; }
    public int Children { get; set; }
}

public class ApplicationDetailsPetDto
{
    public string Type { get; set; } = default!;
    public string Size { get; set; } = default!;
    public string Description { get; set; } = default!;
}
public class ApplicationDetailsCarDto
{
    public string Model { get; set; } = default!;
    public string Make { get; set; } = default!;
    public string Plate { get; set; } = default!;
    public int Year { get; set; }
}

public class TenantDetailDto
{
    public string Name { get; set; } = default!;
    public GetTenantProfileImageDto? ProfileImage { get; set; }
    public bool IsMain { get; set; }
    public bool IsGuarantor { get; set; }
    public GetTenantVerifiedbyProfileDto? Detail { get; set; }
}
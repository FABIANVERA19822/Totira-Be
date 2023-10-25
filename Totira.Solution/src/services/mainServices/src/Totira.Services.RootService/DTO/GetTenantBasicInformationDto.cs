namespace Totira.Services.RootService.DTO;

public class GetTenantBasicInformationDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? SocialInsuranceNumber { get; set; }
    public string AboutMe { get; set; } = default!;
    public TenantBirthday Birthday { get; set; } = default!;
}
public class TenantBirthday
{
    public TenantBirthday(int year, int day, int month)
    {
        this.Year = year;
        this.Day = day;
        this.Month = month;
    }

    public int Year { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
}



using Totira.Bussiness.UserService.Domain;
using static Totira.Bussiness.UserService.Domain.TenantBasicInformation;

namespace Totira.Bussiness.UserService.DTO;

public class GetTenantBasicInformationDto
{
    protected GetTenantBasicInformationDto(Guid id,
        string firstName,
        string lastName,
        string? socialInsuranceNumber,
        string aboutMe,
        TenantBirthdayDto? birthday)
    {

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        SocialInsuranceNumber = socialInsuranceNumber;
        Birthday = birthday;
        AboutMe = aboutMe;
    }

    public GetTenantBasicInformationDto(Guid id)
    {
        Id = id;
        FirstName = default!;
        LastName = default!;
        AboutMe = default!;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SocialInsuranceNumber { get; set; }
    public TenantBirthdayDto? Birthday { get; set; }
    public string AboutMe { get; set; }

    /// <summary>
    /// Creates an empty <see cref="GetTenantBasicInformationDto"/> object only with given tenant id.
    /// </summary>
    /// <param name="tenantId">Tenant id.</param>
    /// <returns>An empty <see cref="GetTenantBasicInformationDto"/> object.</returns>
    public static GetTenantBasicInformationDto Empty(Guid tenantId) => new(tenantId);

    /// <summary>
    /// Adapts a <see cref="TenantBasicInformation"/> entity to <see cref="GetTenantBasicInformationDto"/> data transfer object.
    /// </summary>
    /// <param name="entity">Basic information entity.</param>
    /// <returns>A new <see cref="GetTenantBasicInformationDto"/> data transfer object.</returns>
    public static GetTenantBasicInformationDto AdaptFrom(TenantBasicInformation entity)
        => new(entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.SocialInsuranceNumber,
            entity.AboutMe,
            TenantBirthdayDto.AdaptFrom(entity.Birthday));
}
public class TenantBirthdayDto
{
    protected TenantBirthdayDto(int year, int day, int month)
    {
        this.Year = year;
        this.Day = day;
        this.Month = month;
    }

    public int Year { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }

    /// <summary>
    /// Adapts a <see cref="TenantBirthday"/> property object to <see cref="TenantBirthdayDto"/> data transfer object.
    /// </summary>
    /// <param name="birthday">Birthday object.</param>
    /// <returns>A new <see cref="TenantBirthdayDto"/> data transfer object.</returns>
    public static TenantBirthdayDto? AdaptFrom(TenantBirthday? birthday)
        => birthday is null ? default : new(birthday.Year, birthday.Day, birthday.Month);
}

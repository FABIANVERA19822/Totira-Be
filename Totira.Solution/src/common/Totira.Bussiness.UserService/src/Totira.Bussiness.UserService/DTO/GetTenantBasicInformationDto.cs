using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO;

public class GetTenantBasicInformationDto
{
    protected GetTenantBasicInformationDto(Guid id,
        string firstName,
        string lastName,
        string? socialInsuranceNumber,
        string aboutMe,
        BirthdayDto? birthday)
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
    public BirthdayDto? Birthday { get; set; }
    public string AboutMe { get; set; }

    /// <summary>
    /// Creates an empty <see cref="GetTenantBasicInformationDto"/> object only with given tenant id.
    /// </summary>
    /// <param name="tenantId">Tenant id.</param>
    /// <returns>An empty <see cref="GetTenantBasicInformationDto"/> object.</returns>
    public static GetTenantBasicInformationDto Empty(Guid tenantId) => new(tenantId);

    /// <summary>
    /// Adapts a <see cref="LandlordBasicInformation"/> entity to <see cref="GetTenantBasicInformationDto"/> data transfer object.
    /// </summary>
    /// <param name="entity">Basic information entity.</param>
    /// <returns>A new <see cref="GetTenantBasicInformationDto"/> data transfer object.</returns>
    public static GetTenantBasicInformationDto AdaptFrom(TenantBasicInformation entity)
        => new(entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.SocialInsuranceNumber,
            entity.AboutMe,
            BirthdayDto.AdaptFrom(entity.Birthday));
}

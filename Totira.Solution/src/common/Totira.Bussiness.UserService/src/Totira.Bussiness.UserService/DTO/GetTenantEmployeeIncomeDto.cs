using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO;

public class GetTenantEmployeeIncomeDto
{
    protected GetTenantEmployeeIncomeDto(Guid incomeId,
        string companyOrganizationName,
        string position,
        int? monthlyIncome,
        string startDate,
        string? endDate,
        bool isCurrentlyWorking,
        EmploymentContactReferenceDto contactReference,
        IEnumerable<TenantFileDisplayDto>? files)
    {
        IncomeId = incomeId;
        CompanyOrganizationName = companyOrganizationName;
        Position = position;
        MonthlyIncome = monthlyIncome;
        StartDate = startDate;
        EndDate = endDate;
        IsCurrentlyWorking = isCurrentlyWorking;
        ContactReference = contactReference;
        Files = files;
    }

    /// <summary>
    /// Initializes a new <see cref="GetTenantEmployeeIncomeDto"/> object only with given income id.
    /// </summary>
    /// <param name="incomeId">Income id</param>
    protected GetTenantEmployeeIncomeDto(Guid incomeId)
    {
        IncomeId = incomeId;
        CompanyOrganizationName = default!;
        Position = default!;
        ContactReference = default!;
        StartDate = default!;
        EndDate = default!;
    }

    public Guid IncomeId { get; set; }
    public string CompanyOrganizationName { get; set; }
    public string Position { get; set; }
    public int? MonthlyIncome { get; set; }
    public string StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
    public EmploymentContactReferenceDto ContactReference { get; set; }
    public IEnumerable<TenantFileDisplayDto>? Files { get; set; }

    /// <summary>
    /// Adapts a <see cref="TenantEmployeeIncome"/> object to <see cref="EmployeeIncomeDto" />
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static GetTenantEmployeeIncomeDto AdaptFrom(TenantEmployeeIncome entity)
        => new(
            entity.Id,
            entity.CompanyOrganizationName,
            entity.Position,
            entity.MonthlyIncome,
            entity.StartDate.ToString("yyyy-MM-dd"),
            entity.EndDate?.ToString("yyyy-MM-dd"),
            entity.IsCurrentlyWorking,
            EmploymentContactReferenceDto.AdaptFrom(entity.ContactReference),
            TenantFileDisplayDto.AdaptFrom(entity.Files));

    /// <summary>
    /// Returns a new <see cref="GetTenantEmployeeIncomeDto"/> object only with the given id.
    /// </summary>
    /// <param name="incomeId">Income id.</param>
    /// <returns>An empty new <see cref="GetTenantEmployeeIncomeDto"/> object.</returns>
    public static GetTenantEmployeeIncomeDto Empty(Guid incomeId) => new(incomeId);
}


public class EmploymentContactReferenceDto
{
    protected EmploymentContactReferenceDto(
        string firstName,
        string lastName,
        string jobTitle,
        string relationship,
        string email,
        EmploymentContactReferencePhoneNumberDto phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        Relationship = relationship;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string JobTitle { get; set; }
    public string Relationship { get; set; }
    public string Email { get; set; }
    public EmploymentContactReferencePhoneNumberDto PhoneNumber { get; set; }

    /// <summary>
    /// Adapts a <see cref="EmploymentContactReference"/> object to <see cref="EmploymentContactReferenceDto"/>
    /// </summary>
    /// <param name="contact">Object to adapt.</param>
    /// <returns>A new <see cref="EmploymentContactReferenceDto"/> object.</returns>
    public static EmploymentContactReferenceDto AdaptFrom(EmploymentContactReference contact)
        => new(
            contact.FirstName,
            contact.LastName,
            contact.JobTitle,
            contact.Relationship,
            contact.Email,
            EmploymentContactReferencePhoneNumberDto.AdaptFrom(contact.PhoneNumber));
}

public class EmploymentContactReferencePhoneNumberDto
{
    protected EmploymentContactReferencePhoneNumberDto(string value, string countryCode)
    {
        Value = value;
        CountryCode = countryCode;
    }

    public string Value { get; set; }
    public string CountryCode { get; set; }

    /// <summary>
    /// Adapts a <see cref="EmploymentContactReferencePhoneNumber"/> object to <see cref="EmploymentContactReferencePhoneNumberDto"/>
    /// </summary>
    /// <param name="phoneNumber">Object to adapt.</param>
    /// <returns>A new <see cref="EmploymentContactReferencePhoneNumberDto"/> object.</returns>
    public static EmploymentContactReferencePhoneNumberDto AdaptFrom(EmploymentContactReferencePhoneNumber phoneNumber)
        => new(phoneNumber.Value, phoneNumber.CountryCode);
}

using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain;

public class TenantBasicInformation : Document, IAuditable, IEquatable<TenantBasicInformation>
{
    public TenantBasicInformation()
    {
    }

    /// <summary>
    /// Initializes a new <see cref="TenantBasicInformation"/> instance for creating entity purposes.
    /// </summary>
    /// <param name="id">Tenant id.</param>
    /// <param name="firstName">Tenant first name.</param>
    /// <param name="lastName">Tenant last name.</param>
    /// <param name="birthday">Tenant birthday object.</param>
    /// <param name="socialInsuranceNumber">Tenant social insurance number.</param>
    /// <param name="aboutMe">Tenant "about me" information.</param>
    /// <param name="createdOn">Created on date.</param>
    protected TenantBasicInformation(Guid id,
        string tenantEmail,
        string firstName,
        string lastName,
        TenantBirthday? birthday,
        string? socialInsuranceNumber,
        string aboutMe,
        DateTimeOffset createdOn)
    {
        Id = id;
        TenantEmail = tenantEmail;
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        SocialInsuranceNumber = socialInsuranceNumber;
        AboutMe = aboutMe;
        CreatedOn = createdOn;
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string TenantEmail { get; set; }
    public TenantBirthday? Birthday { get; set; }
    public string? SocialInsuranceNumber { get; set; }
    public string AboutMe { get; set; } = string.Empty;

    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    /// <inheritdoc />
    public bool Equals(TenantBasicInformation? other)
    {
        if (other is null)
            return false;

        if (this.Id != other.Id)
            return false;

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        if (obj is not TenantBasicInformation)
            return false;

        return Equals(obj as TenantBasicInformation);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode() * 41;

    /// <summary>
    /// Updates the tenant basic information
    /// </summary>
    /// <param name="firstName">Tenant first name.</param>
    /// <param name="lastName">Tenant last name.</param>
    /// <param name="birthday">Tenant birthday object.</param>
    /// <param name="socialInsuranceNumber">Tenant social insurance number.</param>
    /// <param name="aboutMe">Tenant "about me" information.</param>
    public void UpdateInformation(string firstName,
        string lastName,
        TenantBirthday birthday,
        string socialInsuranceNumber,
        string aboutMe)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        SocialInsuranceNumber = socialInsuranceNumber;
        AboutMe = aboutMe;
        UpdatedOn = DateTimeOffset.Now;
    }

    /// <summary>
    /// Determines if entity has missing information.
    /// </summary>
    /// <returns>A boolean value.</returns>
    public bool HasMissingInformation() => string.IsNullOrWhiteSpace(FirstName)
        || string.IsNullOrWhiteSpace(LastName)
        || Birthday is null
        || Birthday.Day == 0
        || Birthday.Month == 0
        || Birthday.Year == 0;

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

        /// <summary>
        /// Creates a new <see cref="TenantBirthday"/> object from given year, day and month.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="day">Day.</param>
        /// <param name="month">Month.</param>
        /// <returns>A new <see cref="TenantBirthday"/> object.</returns>
        public static TenantBirthday From(int year, int day, int month) => new(year, day, month);
    }

    /// <summary>
    /// Creates a new <see cref="TenantBasicInformation"/> object from given data.
    /// </summary>
    /// <param name="tenantId">Tenant id.</param>
    /// <param name="firstName">Tenant first name.</param>
    /// <param name="lastName">Tenant last name.</param>
    /// <param name="birthday">Tenant birthday object.</param>
    /// <param name="socialInsuranceNumber">Tenant social insurance number.</param>
    /// <param name="aboutMe">Tenant "about me" information.</param>
    /// <returns>A new <see cref="TenantBasicInformation"/> entity.</returns>
    public static TenantBasicInformation Create(Guid tenantId,
        string tenantEmail,
        string firstName,
        string lastName,
        TenantBirthday? birthday,
        string? socialInsuranceNumber,
        string aboutMe)
        => new(tenantId,
            tenantEmail,
            firstName,
            lastName,
            birthday,
            socialInsuranceNumber,
            aboutMe,
            DateTimeOffset.Now);
}

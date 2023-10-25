using MongoDB.Bson.Serialization.Attributes;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public sealed class TenantEmployeeIncome : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        /// <summary>
        /// Initalizes a new instance of <see cref="TenantEmployeeIncome"/> class.
        /// </summary>
        /// <param name="tenantId">Tenant Id.</param>
        /// <param name="companyOrganizationName">Employment company or organization name.</param>
        /// <param name="position">Employement position.</param>
        /// <param name="startDate">Employment start date.</param>
        /// <param name="isCurrentlyWorking">Indicates if is currently working there.</param>
        /// <param name="endDate">Employement end date.</param>
        /// <param name="monthlyIncome">Employment monthtly income.</param>
        /// <param name="contactReference">Employement contact reference.</param>
        /// <param name="files">Employee incomes files.</param>
        /// <param name="createdOn">Created on date.</param>
        private TenantEmployeeIncome(
            Guid id,
            Guid tenantId,
            string companyOrganizationName,
            string position,
            DateTime startDate,
            bool isCurrentlyWorking,
            DateTime? endDate,
            int? monthlyIncome,
            string status,
            EmploymentContactReference contactReference,
            List<TenantFile> files,
            DateTimeOffset createdOn)
        {
            Id = id;
            TenantId = tenantId;
            CompanyOrganizationName = companyOrganizationName;
            Position = position;
            StartDate = startDate;
            IsCurrentlyWorking = isCurrentlyWorking;
            EndDate = endDate;
            MonthlyIncome = monthlyIncome;
            Status = status;
            ContactReference = contactReference;
            Files = files;
            CreatedOn = createdOn;
        }

        public Guid TenantId { get; set; }
        public string CompanyOrganizationName { get; set; }
        public string Position { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime StartDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime? EndDate { get; set; }
        public int? MonthlyIncome { get; set; }
        public string Status { get; set; }
        public EmploymentContactReference ContactReference { get; set; }
        public List<TenantFile> Files { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new <see cref="TenantEmployeeIncome"/> object.
        /// </summary>
        /// <param name="id">New instance id given because of file creation needed before.</param>
        /// <param name="tenantId">Tenant ID.</param>
        /// <param name="companyOrganizationName">Employment company or organization name.</param>
        /// <param name="position">Employment position.</param>
        /// <param name="startDate">Employment start date.</param>
        /// <param name="isCurrentlyWorking">Employee still currently working there?</param>
        /// <param name="endDate">Employment end date if is not currently working there.</param>
        /// <param name="monthlyIncome">Employment monthly income.</param>
        /// <param name="status">Employee income proof status.</param>
        /// <param name="files">Files as prof of employee income.</param>
        /// <param name="contactReference">Employment contact reference.</param>
        /// <returns>A new <see cref="TenantEmployeeIncome"/> object.</returns>
        public static TenantEmployeeIncome Create(
            Guid id,
            Guid tenantId,
            string companyOrganizationName,
            string position,
            DateTime startDate,
            bool isCurrentlyWorking,
            DateTime? endDate,
            int? monthlyIncome,
            EmploymentContactReference contactReference,
            IList<TenantFile> files)
        => new(
            id,
            tenantId,
            companyOrganizationName,
            position,
            startDate,
            isCurrentlyWorking,
            endDate,
            monthlyIncome,
            "under revision",
            contactReference,
            files.ToList(),
            DateTimeOffset.Now);

        /// <summary>
        /// Updates body information.
        /// </summary>
        /// <param name="companyOrganizationName">Employment company or organization name.</param>
        /// <param name="position">Employement position.</param>
        /// <param name="startDate">Employment start date.</param>
        /// <param name="isCurrentlyWorking">Indicates if is currently working there.</param>
        /// <param name="endDate">Employement end date.</param>
        /// <param name="monthlyIncome">Employment monthtly income.</param>
        /// <param name="incomeId">Income Id.</param>
        public void UpdateInformation(string companyOrganizationName,
            string position,
            DateTime startDate,
            bool isCurrentlyWorking,
            DateTime? endDate,
            int? monthlyIncome,
            Guid incomeId)
        {
            CompanyOrganizationName = companyOrganizationName;
            Position = position;
            MonthlyIncome = monthlyIncome;
            StartDate = startDate;
            IsCurrentlyWorking = isCurrentlyWorking;
            EndDate = endDate;
            Id = incomeId;
            UpdatedOn = DateTimeOffset.Now;

            if (Status != "under revision")
                Status = "under revision";
        }

        /// <summary>
        /// Add a new file info to store in database.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="key">Aws S3 Key.</param>
        public void AddNewFileInfo(string fileName, string key, string contentType, int size)
            => Files.Add(TenantFile.Create(fileName, key, contentType, size));
    }

    public class EmploymentContactReference
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EmploymentContactReference"/> class.
        /// </summary>
        /// <param name="firstName">Contact first name.</param>
        /// <param name="lastName">Contact last name</param>
        /// <param name="jobTitle">Contact job title.</param>
        /// <param name="relationship">Contact relationship.</param>
        /// <param name="email">Contact email.</param>
        /// <param name="phoneNumber">Contact phone number.</param>
        protected EmploymentContactReference(
            string firstName,
            string lastName,
            string jobTitle,
            string relationship,
            string email,
            EmploymentContactReferencePhoneNumber phoneNumber)
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
        public EmploymentContactReferencePhoneNumber PhoneNumber { get; set; }

        /// <summary>
        /// Creates a new <see cref="EmploymentContactReference" /> object.
        /// </summary>
        /// <param name="firstName">Contact first name.</param>
        /// <param name="lastName">Contact last name</param>
        /// <param name="jobTitle">Contact job title.</param>
        /// <param name="relationship">Contact relationship.</param>
        /// <param name="email">Contact email.</param>
        /// <param name="phoneNumber">Contact phone number.</param>
        /// <returns>A <see cref="EmploymentContactReference"/> new object.</returns>
        public static EmploymentContactReference Create(
            string firstName,
            string lastName,
            string jobTitle,
            string relationship,
            string email,
            EmploymentContactReferencePhoneNumber phoneNumber)
            => new(firstName, lastName, jobTitle, relationship, email, phoneNumber);

        /// <summary>
        /// Updates body information.
        /// </summary>
        /// <param name="firstName">Contact first name.</param>
        /// <param name="lastName">Contact last name</param>
        /// <param name="jobTitle">Contact job title.</param>
        /// <param name="relationship">Contact relationship.</param>
        /// <param name="email">Contact email.</param>
        public void UpdateInformation(
            string firstName,
            string lastName,
            string jobTitle,
            string relationship,
            string email)
        {
            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle;
            Relationship = relationship;
            Email = email;
        }
    }

    public class EmploymentContactReferencePhoneNumber
    {
        /// <summary>
        /// Creates a new <see cref="EmploymentContactReferencePhoneNumber"/> object.
        /// </summary>
        /// <param name="value">Contact phone number value.</param>
        /// <param name="countryCode">Contact phone number country code.</param>
        /// <returns>A new <see cref="EmploymentContactReferencePhoneNumber"/> object.</returns>
        public static EmploymentContactReferencePhoneNumber Create(string value, string countryCode)
            => new(value, countryCode);

        /// <summary>
        /// Initializes a new instance of <see cref="EmploymentContactReferencePhoneNumber"/> class.
        /// </summary>
        /// <param name="value">Contact phone number value.</param>
        /// <param name="countryCode">Contact phone number country code.</param>
        protected EmploymentContactReferencePhoneNumber(string value, string countryCode)
        {
            Value = value;
            CountryCode = countryCode;
        }
        /// <summary>
        /// Contact phone number value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Contact phone number country code.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Updates body information.
        /// </summary>
        /// <param name="value">Contact phone number value.</param>
        /// <param name="countryCode">Contact phone number country code.</param>
        public void UpdateInformation(string value, string countryCode)
        {
            Value = value;
            CountryCode = countryCode;
        }
    }
}

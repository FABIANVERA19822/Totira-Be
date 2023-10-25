using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain.Landlords
{
    public partial class LandlordBasicInformation : Document, IAuditable, IEquatable<LandlordBasicInformation>
    {
        public LandlordBasicInformation()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="Domain.LandlordBasicInformation"/> instance for creating entity purposes.
        /// </summary>
        /// <param name="id">Tenant id.</param>
        /// <param name="firstName">Tenant first name.</param>
        /// <param name="lastName">Tenant last name.</param>
        /// <param name="birthday">Tenant birthday object.</param>
        /// <param name="socialInsuranceNumber">Tenant social insurance number.</param>
        /// <param name="aboutMe">Tenant "about me" information.</param>
        /// <param name="createdOn">Created on date.</param>
        protected LandlordBasicInformation(Guid id,
            string firstName,
            string lastName,
            string email,
            Birthday? birthday,
            string? socialInsuranceNumber,
            string aboutMe,
            DateTimeOffset createdOn)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            LandlordBirthday = birthday;
            SocialInsuranceNumber = socialInsuranceNumber;
            AboutMe = aboutMe;
            CreatedOn = createdOn;
        }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Birthday? LandlordBirthday { get; set; }
        public string? SocialInsuranceNumber { get; set; }
        public string AboutMe { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        /// <inheritdoc />
        public bool Equals(LandlordBasicInformation? other)
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

            if (obj is not LandlordBasicInformation)
                return false;

            return Equals(obj as LandlordBasicInformation);
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
            string email,
            Birthday birthday,
            string socialInsuranceNumber,
            string aboutMe)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            LandlordBirthday = birthday;
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
            || string.IsNullOrWhiteSpace(SocialInsuranceNumber)
            || LandlordBirthday is null
            || LandlordBirthday.Day == 0
            || LandlordBirthday.Month == 0
            || LandlordBirthday.Year == 0;

        /// <summary>
        /// Creates a new <see cref="Domain.LandlordBasicInformation"/> object from given data.
        /// </summary>
        /// <param name="landlordId">Tenant id.</param>
        /// <param name="firstName">Tenant first name.</param>
        /// <param name="lastName">Tenant last name.</param>
        /// <param name="birthday">Tenant birthday object.</param>
        /// <param name="socialInsuranceNumber">Tenant social insurance number.</param>
        /// <param name="aboutMe">Tenant "about me" information.</param>
        /// <returns>A new <see cref="Domain.LandlordBasicInformation"/> entity.</returns>
        public static LandlordBasicInformation Create(Guid landlordId,
            string firstName,
            string lastName,
            string email,
            Birthday? birthday,
            string? socialInsuranceNumber,
            string aboutMe)
            => new(landlordId,
                firstName,
                lastName,
                email,
                birthday,
                socialInsuranceNumber,
                aboutMe,
                DateTimeOffset.Now);
    }
}

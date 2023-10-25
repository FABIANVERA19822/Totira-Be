using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantContactInformation : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid TenantId { get; set; }
        public string HousingStatus { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string Unit { get; set; }
        public string Email { get; set; } = string.Empty;
        public ContactInformationPhoneNumber PhoneNumber { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if entity has missing information
        /// </summary>
        /// <returns>A boolean value.</returns>
        public bool HasMissingInformation() => string.IsNullOrWhiteSpace(StreetAddress)
            || string.IsNullOrWhiteSpace(City)
            || string.IsNullOrWhiteSpace(Country)
            || string.IsNullOrWhiteSpace(Province);
    }
}

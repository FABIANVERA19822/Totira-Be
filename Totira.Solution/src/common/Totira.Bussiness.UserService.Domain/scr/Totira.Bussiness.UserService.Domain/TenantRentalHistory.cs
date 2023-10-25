using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantRentalHistory : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid TenantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public CustomDate? RentalStartDate { get; set; }
        public bool CurrentlyLivingHere { get; set; }
        public CustomDate? RentalEndDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public bool IsFeedbackRequest { get; set; }
        public string Status { get; set; } = string.Empty;

        public RentalHistoryLandlordContactInformation? ContactInformation { get; set; }


        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
    public class RentalHistoryLandlordContactInformation
    {
        public RentalHistoryLandlordContactInformation(string relationship, string firstName, string lastName, RentalHistoriesPhoneNumber phoneNumber, string emailAddress)
        {
            Relationship = relationship;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }
        public string Relationship { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public RentalHistoriesPhoneNumber PhoneNumber { get; set; }
        public string EmailAddress { get; set; } = string.Empty;

    }
    public class CustomDate
    {
        public CustomDate(int month, int year)
        {
            Month = month;
            Year = year;
        }

        public int Month { get; set; }
        public int Year { get; set; }
    }
    public class RentalHistoriesPhoneNumber
    {
        public RentalHistoriesPhoneNumber(string phoneNumper, string countryCode)
        {
            Number = phoneNumper;
            CountryCode = countryCode;
        }

        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }


}


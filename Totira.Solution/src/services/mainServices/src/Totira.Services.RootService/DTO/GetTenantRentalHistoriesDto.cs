namespace Totira.Services.RootService.DTO
{
    public class GetTenantRentalHistoriesDto
    {
        public Guid TenantId { get; set; }
        public List<TenantRentalHistoryDto>? RentalHistories { get; set; }

    }
    public class TenantRentalHistoryDto
    {
        public TenantRentalHistoryDto(Guid documentId, CustomDate rentalStartDate, bool currentlyLivingHere, CustomDate rentalEndDate,
           string country, string state, string city, string address, string unit, string zipCode, string status,
           LandlordContactInformation? contactInformation)
        {
            DocumentId = documentId;
            RentalStartDate = rentalStartDate;
            CurrentlyLivingHere = currentlyLivingHere;
            RentalEndDate = rentalEndDate;
            Country = country;
            State = state;
            City = city;
            Address = address;
            Unit = unit;
            ZipCode = zipCode;
            ContactInformation = contactInformation;
            Status = status;

        }
        public Guid DocumentId { get; set; }
        public CustomDate RentalStartDate { get; set; }
        public bool CurrentlyLivingHere { get; set; }
        public CustomDate RentalEndDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Status { get; set; }
        public LandlordContactInformation? ContactInformation { get; set; }


    }
    public class LandlordContactInformation
    {
        public LandlordContactInformation(string relationship, string firstName, string lastName, RentalHistoriesPhoneNumber phoneNumber, string emailAddress)
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


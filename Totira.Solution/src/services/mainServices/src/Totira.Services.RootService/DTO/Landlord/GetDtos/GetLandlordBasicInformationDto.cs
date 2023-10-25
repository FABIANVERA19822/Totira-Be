namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetLandlordBasicInformationDto
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? SocialInsuranceNumber { get; set; }
        public string AboutMe { get; set; } = default!;
        public LandlordBirthday Birthday { get; set; } = default!;
    }
    public class LandlordBirthday
    {
        public LandlordBirthday(int year, int day, int month)
        {
            Year = year;
            Day = day;
            Month = month;
        }

        public int Year { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}

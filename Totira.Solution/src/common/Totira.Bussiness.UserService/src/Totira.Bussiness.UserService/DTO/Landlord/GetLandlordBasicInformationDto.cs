
using static Totira.Bussiness.UserService.Domain.Landlords.LandlordBasicInformation;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Domain.Common;

namespace Totira.Bussiness.UserService.DTO.Landlord
{
    public class GetLandlordBasicInformationDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? SocialInsuranceNumber { get; set; }
        public string AboutMe { get; set; } = default!;
        public LandlordBirthdayDto Birthday { get; set; } = default!;

        protected GetLandlordBasicInformationDto(Guid id,
            string firstName,
            string lastName,
            string? socialInsuranceNumber,
            string? aboutMe,
            LandlordBirthdayDto? birthday)
        {

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            SocialInsuranceNumber = socialInsuranceNumber;
            Birthday = birthday;
            AboutMe = aboutMe;
        }
        public GetLandlordBasicInformationDto(Guid id)
        {
            Id = id;
            FirstName = default!;
            LastName = default!;
            AboutMe = default!;
        }

        /// <summary>
        /// Creates an empty <see cref="GetLandlordBasicInformationDto"/> object only with given Landlord id.
        /// </summary>
        /// <param name="LandlordId">Landlord id.</param>
        /// <returns>An empty <see cref="GetLandlordBasicInformationDto"/> object.</returns>
        public static GetLandlordBasicInformationDto Empty(Guid LandlordId) => new(LandlordId);

        /// <summary>
        /// Adapts a <see cref="LandlordBasicInformation"/> entity to <see cref="GetLandlordBasicInformationDto"/> data transfer object.
        /// </summary>
        /// <param name="entity">Basic information entity.</param>
        /// <returns>A new <see cref="GetLandlordBasicInformationDto"/> data transfer object.</returns>
        public static GetLandlordBasicInformationDto AdaptFrom(LandlordBasicInformation entity)
            => new(entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.SocialInsuranceNumber,
                entity.AboutMe,
                LandlordBirthdayDto.AdaptFrom(entity.LandlordBirthday));
    }
    public class LandlordBirthdayDto
    {
        public LandlordBirthdayDto(int year, int day, int month)
        {
            this.Year = year;
            this.Day = day;
            this.Month = month;
        }

        public int Year { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }


        /// <summary>
        /// Adapts a <see cref="Birthday"/> property object to <see cref="LandlordBirthdayDto"/> data transfer object.
        /// </summary>
        /// <param name="birthday">Birthday object.</param>
        /// <returns>A new <see cref="LandlordBirthdayDto"/> data transfer object.</returns>
        public static LandlordBirthdayDto? AdaptFrom(Birthday? birthday)
            => birthday is null ? default : new(birthday.Year, birthday.Day, birthday.Month);
    }

}

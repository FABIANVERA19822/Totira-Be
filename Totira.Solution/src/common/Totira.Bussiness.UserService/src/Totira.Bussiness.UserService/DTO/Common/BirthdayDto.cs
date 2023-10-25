using Totira.Bussiness.UserService.Domain.Common;

namespace Totira.Bussiness.UserService.DTO.Common;

public class BirthdayDto
{
    protected BirthdayDto(int year, int day, int month)
    {
        Year = year;
        Day = day;
        Month = month;
    }

    public int Year { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }

    /// <summary>
    /// Adapts a <see cref="TenantBirthday"/> property object to <see cref="BirthdayDto"/> data transfer object.
    /// </summary>
    /// <param name="birthday">Birthday object.</param>
    /// <returns>A new <see cref="BirthdayDto"/> data transfer object.</returns>
    public static BirthdayDto? AdaptFrom(Birthday? birthday)
        => birthday is null ? default : new(birthday.Year, birthday.Day, birthday.Month);
}

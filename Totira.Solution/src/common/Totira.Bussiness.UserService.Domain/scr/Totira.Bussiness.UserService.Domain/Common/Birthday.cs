namespace Totira.Bussiness.UserService.Domain.Common
{
    public class Birthday
    {
        public Birthday(int year, int day, int month)
        {
            this.Year = year;
            this.Day = day;
            this.Month = month;
        }

        public int Year { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }

        /// <summary>
        /// Creates a new <see cref="Birthday"/> object from given year, day and month.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="day">Day.</param>
        /// <param name="month">Month.</param>
        /// <returns>A new <see cref="Birthday"/> object.</returns>
        public static Birthday From(int year, int day, int month) => new(year, day, month);
    }
}


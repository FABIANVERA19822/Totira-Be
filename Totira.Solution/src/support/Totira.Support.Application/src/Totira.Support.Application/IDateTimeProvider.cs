namespace Totira.Support.Application
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}

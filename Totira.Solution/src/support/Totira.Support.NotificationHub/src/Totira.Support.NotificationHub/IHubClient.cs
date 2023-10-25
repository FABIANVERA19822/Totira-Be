namespace Totira.Support.NotificationHub
{
    public interface IHubClient
    {
        Task Notify(NotificationMessage message);
    }
}

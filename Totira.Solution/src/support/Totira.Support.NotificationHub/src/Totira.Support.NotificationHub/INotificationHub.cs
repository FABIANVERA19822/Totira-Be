namespace Totira.Support.NotificationHub
{
    public interface INotificationHub
    {
        Task SendAsync(string userId, NotificationMessage message);


    }
}

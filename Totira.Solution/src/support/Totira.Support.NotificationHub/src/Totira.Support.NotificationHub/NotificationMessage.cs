namespace Totira.Support.NotificationHub
{
    public class NotificationMessage
    {
        public string Status { get; }
        public string Event { get; set; }
        public string Message { get; }

        public NotificationMessage(NotificationMessageStatus status, string message)
        {
            Status = status.ToString().ToLowerInvariant();
            Message = message;
        }
    }
}
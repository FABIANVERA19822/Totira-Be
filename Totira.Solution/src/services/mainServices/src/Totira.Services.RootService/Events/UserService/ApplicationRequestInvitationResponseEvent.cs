using Totira.Support.Application.Events;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    public class ApplicationRequestInvitationResponseEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public ApplicationRequestInvitationResponseEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new ApplicationRequestInvitationResponseInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class ApplicationRequestInvitationResponseInfo
        {
            private Guid id;

            public ApplicationRequestInvitationResponseInfo(Guid id)
            {
                this.id = id;
            }
        }
    }
}

using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira
{
    [RoutingKey("CreateLandlordPropertyClaimsJiraTicketEvent")]
    public class CreateLandlordPropertyClaimsJiraTicketEvent : BaseValidatedEvent, INotification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public CreateLandlordPropertyClaimsJiraTicketEvent()
        {
            this.Id = Guid.Empty;
            this.Message = string.Empty;
        }

        public CreateLandlordPropertyClaimsJiraTicketEvent(Guid id, string Message)
        {
            this.Id = id;
            this.Message = Message;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var status = NotificationMessageStatus.Success;
            var json = string.Empty;
            if (IsValid)
            {
                var info = new CreateLandlordPropertyClaimsJiraTicketInfo(Id, Message);
                json = System.Text.Json.JsonSerializer.Serialize(info);
            }
            else
            {
                status = NotificationMessageStatus.Error;
                json = System.Text.Json.JsonSerializer.Serialize(Errors);
            }

            return new NotificationMessage(status, json);
        }

        private class CreateLandlordPropertyClaimsJiraTicketInfo
        {
            public Guid Id { get; }
            public string Message { get; }

            public CreateLandlordPropertyClaimsJiraTicketInfo(Guid id, string message)
            {
                this.Id = id;
                this.Message = message;
            }
        }
    }
}
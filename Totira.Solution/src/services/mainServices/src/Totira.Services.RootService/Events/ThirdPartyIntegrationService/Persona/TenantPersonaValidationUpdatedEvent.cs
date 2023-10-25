using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira
{  

    [RoutingKey("TenantPersonaValidationUpdatedEvent")]
    public class TenantPersonaValidationUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantPersonaValidationUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantPersonaValidationUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantPersonaValidationUpdatedInfo
        {
            private Guid id;

            public TenantPersonaValidationUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

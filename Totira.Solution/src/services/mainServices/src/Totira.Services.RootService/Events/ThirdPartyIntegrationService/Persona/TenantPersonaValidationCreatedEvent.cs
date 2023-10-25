using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira
{

    [RoutingKey("TenantPersonaValidationCreatedEvent")]
    public class TenantPersonaValidationCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantPersonaValidationCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantPersonaValidationCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantPersonaValidationCreatedInfo
        {
            private Guid id;

            public TenantPersonaValidationCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}

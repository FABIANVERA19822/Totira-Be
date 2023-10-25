using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Events.Persona
{  

    [RoutingKey("TenantPersonaValidationUpdatedEvent")]
    public class TenantPersonaValidationUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantPersonaValidationUpdatedEvent(Guid id) => Id = id;

    }
}

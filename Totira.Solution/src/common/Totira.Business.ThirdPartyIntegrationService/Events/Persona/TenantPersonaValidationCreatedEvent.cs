using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Events.Persona
{

    [RoutingKey("TenantPersonaValidationCreatedEvent")]
    public class TenantPersonaValidationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantPersonaValidationCreatedEvent(Guid id) => Id = id;

    }
}

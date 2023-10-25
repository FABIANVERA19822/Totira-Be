using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantContactLandlordCreatedEvent")]
    public class TenantContactLandlordCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantContactLandlordCreatedEvent(Guid id) => Id = id;
    }
}


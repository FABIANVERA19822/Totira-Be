using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantShareProfileCreatedEvent")]
    public class TenantShareProfileCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantShareProfileCreatedEvent(Guid id) => Id = id;
    }
}


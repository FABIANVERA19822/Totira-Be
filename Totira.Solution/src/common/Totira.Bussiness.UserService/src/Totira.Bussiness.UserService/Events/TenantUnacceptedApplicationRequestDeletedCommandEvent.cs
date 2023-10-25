using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantUnacceptedApplicationRequestDeletedCommandEvent")]
    public class TenantUnacceptedApplicationRequestDeletedCommandEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantUnacceptedApplicationRequestDeletedCommandEvent(Guid id) => Id = id;
    }
}


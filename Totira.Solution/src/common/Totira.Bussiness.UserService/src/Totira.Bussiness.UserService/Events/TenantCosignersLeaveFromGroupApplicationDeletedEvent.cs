using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantCosignersLeaveFromGroupApplicationDeletedEvent")]
    public class TenantCosignersLeaveFromGroupApplicationDeletedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantCosignersLeaveFromGroupApplicationDeletedEvent(Guid id) => Id = id;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantPropertyApplicationCreatedEvent")]
    public class TenantPropertyApplicationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantPropertyApplicationCreatedEvent(Guid id) => Id = id;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("AcceptTermsAndConditionsCreatedEvent")]
    public class AcceptTermsAndConditionsCreatedEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public AcceptTermsAndConditionsCreatedEvent(Guid id) => Id = id;
        public AcceptTermsAndConditionsCreatedEvent()
        {
            Id = Guid.Empty;
        }
    }
}

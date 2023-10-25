using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("AcceptTermsAndConditionsCreatedEvent")]
    public class AcceptTermsAndConditionsCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public AcceptTermsAndConditionsCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new AcceptTermsAndConditionsInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class AcceptTermsAndConditionsInfo
        {
            private Guid id;        

            public AcceptTermsAndConditionsInfo(Guid id)
            {
                this.id = id;
                
            }
        }
    }
}

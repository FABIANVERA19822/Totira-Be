using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantStudentFinancialDetailFileDeletedEvent")]
    public class TenantStudentFinancialDetailFileDeletedEvent: IEvent
    {
        public Guid Id { get; set; }
        public TenantStudentFinancialDetailFileDeletedEvent(Guid id) => Id = id;
    }
}

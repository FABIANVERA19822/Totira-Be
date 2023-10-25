using System;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("DeleteCosignersFromGroupApplicationCommand")]
    public class DeleteCosignersFromGroupApplicationCommand : ICommand
    {
        public Guid MainTenantId { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public Guid CoSignerId { get; set; }
    }
}


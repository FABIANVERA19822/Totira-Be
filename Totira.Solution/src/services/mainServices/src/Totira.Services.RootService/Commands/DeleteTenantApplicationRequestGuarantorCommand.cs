﻿using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("DeleteTenantApplicationRequestGuarantorCommand")]
    public class DeleteTenantApplicationRequestGuarantorCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public Guid GuarantorId { get; set; }
    }
}

﻿using System;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("DeleteTenantCoSignerFromGroupApplicationProfileCommand")]
    public class DeleteTenantCoSignerFromGroupApplicationProfileCommand : ICommand
    {
        public Guid MainTenantId { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public Guid CoSignerId { get; set; }
    }
}


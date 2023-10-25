﻿using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(ApproveApplicationRequestCommand))]
public class ApproveApplicationRequestCommand : ICommand
{
    /// <summary>
    /// Application request Id
    /// </summary>
    public Guid ApplicationRequestId { get; set; }
    /// <summary>
    /// Tenant Id for single tenant or Main Tenant Id for multitenant
    /// </summary>
    public Guid TenantId { get; set; }
    /// <summary>
    /// Property Id
    /// </summary>
    public string PropertyId { get; set; } = default!;
}
﻿using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.LandlordCommands;

[RoutingKey(nameof(RejectApplicationRequestCommand))]
public class RejectApplicationRequestCommand : ICommand
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    public Guid PropertyApplicationId { get; set; }
}

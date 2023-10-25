﻿using System.Reflection.Metadata.Ecma335;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{

    [RoutingKey("UpdateTenantApplicationRequestCommand")]
    public class UpdateTenantApplicationRequestCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public bool? ToLatest { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? ApplicationDetailsId { get; set; }
        public List<Coapplicant> Coapplicants { get; set; }
        public Guarantor Guarantor { get; set; }
    }

    public class Coapplicant
    {
        public string Email { get; set; }        

    }

    public class Guarantor
    {
        public string Email { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.DTO;
using System.ComponentModel.DataAnnotations;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateGroupApplicationCommand")]
    public class CreateGroupApplicationCommand : ICommand
    {
        public CreateGroupApplicationCommand()
        {
            GroupApplicationProfiles = new List<TenantGroupApplicationProfileDto>();
        }
        public List<TenantGroupApplicationProfileDto> GroupApplicationProfiles { get; set; }
    }
}

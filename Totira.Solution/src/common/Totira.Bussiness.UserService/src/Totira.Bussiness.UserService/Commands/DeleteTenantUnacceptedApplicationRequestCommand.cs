using System;
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("DeleteTenantUnacceptedApplicationRequestCommand")]
    public class DeleteTenantUnacceptedApplicationRequestCommand : ICommand
    {
        [Required(ErrorMessage = "ApplicationRequestId is required.")]
        public Guid RequestId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;
    }
}


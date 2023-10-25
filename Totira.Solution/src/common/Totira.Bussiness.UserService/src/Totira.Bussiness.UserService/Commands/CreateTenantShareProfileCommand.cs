using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantShareProfileCommand")]
    public class CreateTenantShareProfileCommand : ICommand
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "TypeOfContact is required.")]
        public string TypeOfContact { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Property Street Address cannot exceed 150 characters.")]
        public string? PropertyStreetAddress { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Message cannot exceed 255 characters.")]
        public string? Message { get; set; } = string.Empty;

    }
}


using System;
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateTenantShareProfileCommand")]
    public class CreateTenantShareProfileCommand : ICommand
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        
        public string TypeOfContact { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Property Street Address cannot exceed 150 characters.")]
        public string? PropertyStreetAddress { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Message cannot exceed 255 characters.")]
        public string? Message { get; set; } = string.Empty;

    }
}


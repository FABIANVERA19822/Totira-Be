using System;
using System.ComponentModel.DataAnnotations;
using Totira.Bussiness.UserService.Commands.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantContactLandlordCommand")]
    public class CreateTenantContactLandlordCommand : ICommand
    {
        public Guid? TenantId { get; set; }

        [Required(ErrorMessage = "Property Id is required.")]
        public string PropertyId { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(70, ErrorMessage = "FirstName cannot exceed 70 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(70, ErrorMessage = "LastName cannot exceed 70 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required.")]
        public ContactInformationPhoneNumber PhoneNumber { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters.")]
        public string Message { get; set; } = string.Empty;
    }
}


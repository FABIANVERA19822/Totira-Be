using System.ComponentModel.DataAnnotations;
using Totira.Bussiness.UserService.Commands.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantContactInfoCommand")]
    public class CreateTenantContactInformationCommand : ICommand
    {
        public Guid TenantId { get; set; }
        [Required(ErrorMessage = "Housing Status is required.")]
        public string HousingStatus { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string SelectedCountry { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(70, ErrorMessage = "Province cannot exceed 70 characters.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(70, ErrorMessage = "City cannot exceed 70 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "ZIP Code is required.")]
        [StringLength(12, ErrorMessage = "ZIP Code cannot exceed 12 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "ZIP Code should be alphanumeric")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(100, ErrorMessage = "Street Address cannot exceed 100 characters.")]
        public string StreetAddress { get; set; }

        [StringLength(70, ErrorMessage = "Unit cannot exceed 70 characters.")]
        public string Unit { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ContactInformationPhoneNumber? PhoneNumber { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);
    }
}

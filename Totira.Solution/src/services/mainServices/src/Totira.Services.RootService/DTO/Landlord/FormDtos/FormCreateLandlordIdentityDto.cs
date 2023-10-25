using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Totira.Services.RootService.Attributes;
using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO.Landlord.FormDtos
{
    public class FormCreateLandlordIdentityDto
    {
        [Required(ErrorMessage = "Landlord id is required.")]
        [Description("Landlord Id")]
        public Guid LandlordId { get; set; }

        [Required(ErrorMessage = "Identity proof is required.")]
        [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
        [FileSize(20 * 1024 * 1024, 2048, ErrorMessage = "The file exceeds the supported size. Please try again with a smaller file.")]
        public List<IFormFile> IdentityProof { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public ContactInformationPhoneNumberDto ContactInformationPhoneNumber { get; set; } = new ContactInformationPhoneNumberDto(string.Empty, string.Empty);

    }
}

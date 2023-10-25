using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Totira.Services.RootService.Attributes;

namespace Totira.Services.RootService.DTO.Landlord.FormDtos
{
    public class FormPropertyClaimDetailDto
    {
        public string MlsId { get; set; }
        [StringLength(100, ErrorMessage = "Street Address cannot exceed 100 characters.")]
        public string Address { get; set; }
        public string Unit { get; set; }

        [StringLength(70, ErrorMessage = "City cannot exceed 70 characters.")]
        public string City { get; set; }
        public string ListingUrl { get; set; }

        [FileSize(20 * 1024 * 1024, 2048, ErrorMessage = "The ownership proof file exceeds the supported size. Please try again with a smaller file.")]
        [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
        public List<IFormFile>? OwnershipProofs { get; set; }

    }
}

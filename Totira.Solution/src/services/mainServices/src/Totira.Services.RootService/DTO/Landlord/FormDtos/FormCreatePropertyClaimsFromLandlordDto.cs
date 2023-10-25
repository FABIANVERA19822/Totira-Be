using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Totira.Services.RootService.Attributes;
using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO.Landlord.FormDtos
{
    public class FormCreatePropertyClaimsFromLandlordDto
    {
        [Required(ErrorMessage = "Landlord id is required.")]
        [Description("Landlord Id")]
        public Guid LandlordId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        public List<FormPropertyClaimDetailDto> PropertyClaims { get; set; }


    }
}

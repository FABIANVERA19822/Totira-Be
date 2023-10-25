using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO.Files.Request
{
    public class GetLandlordFileRequest
    {
        [Required]
        public string FileName { get; set; } = default!;
        [Required]
        public Guid LandlordId { get; set; }
    }
}

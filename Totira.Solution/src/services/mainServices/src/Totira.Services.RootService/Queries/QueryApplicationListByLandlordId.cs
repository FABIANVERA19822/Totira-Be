using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations; 
using Totira.Services.RootService.DTO.Landlord.GetDtos;

namespace Totira.Services.RootService.Queries
{
    [JsonObject]
    public class QueryApplicationListByLandlordId
    {
        public SortApplicationsBy SortBy { get; set; } = SortApplicationsBy.SubmissionDate;
        public bool Asc { get; set; }
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }
        [Required]
        public Guid LandlordId { get; set; }
        [Required]
        public string PropertyId { get; set; }
    }
}

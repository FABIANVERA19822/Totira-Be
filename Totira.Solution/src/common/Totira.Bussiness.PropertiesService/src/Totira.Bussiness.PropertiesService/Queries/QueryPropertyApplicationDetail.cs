using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.PropertiesService.Queries;

public class QueryPropertyApplicationDetail : IQuery
{
    [Required]
    public string PropertyId { get; set; } = default!;

    public QueryPropertyApplicationDetail(string propertyId)
    {
        PropertyId = propertyId;
    }
}

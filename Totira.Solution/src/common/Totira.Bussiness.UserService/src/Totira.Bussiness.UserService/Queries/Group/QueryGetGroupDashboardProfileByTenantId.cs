using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Group;

public class QueryGetGroupDashboardProfileByTenantId : IQuery
{
    [Required]
    public Guid TenantId { get; set; }
}
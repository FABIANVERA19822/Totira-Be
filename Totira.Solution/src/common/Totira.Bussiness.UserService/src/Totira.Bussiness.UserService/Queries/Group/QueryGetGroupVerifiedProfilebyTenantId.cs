using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Group;

public class QueryGetGroupVerifiedProfilebyTenantId : IQuery
{
    [Required]
    public Guid TenantId { get; set; }
}
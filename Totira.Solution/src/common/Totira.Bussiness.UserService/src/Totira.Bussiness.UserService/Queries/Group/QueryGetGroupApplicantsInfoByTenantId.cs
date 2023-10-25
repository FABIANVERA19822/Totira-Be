using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Group;

public class QueryGetGroupApplicantsInfoByTenantId : IQuery
{
    public Guid TenantId { get; set; }
}
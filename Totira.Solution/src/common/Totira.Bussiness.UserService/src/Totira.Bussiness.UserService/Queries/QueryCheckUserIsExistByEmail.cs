using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public class QueryCheckUserIsExistByEmail : IQuery
{
    public QueryCheckUserIsExistByEmail(string email) => Email = email;
    public string Email { get; } = string.Empty;

}

using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryUserByAdditionalInfo : IQuery
    {
        public QueryUserByAdditionalInfo(string info) => Info = info;

        public string Info { get; }
    }
}

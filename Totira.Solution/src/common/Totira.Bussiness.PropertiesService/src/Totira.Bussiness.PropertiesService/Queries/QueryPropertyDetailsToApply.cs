using Totira.Support.Application.Queries;

namespace Totira.Bussiness.PropertiesService.Queries
{
    public class QueryPropertyDetailsToApply : IQuery
    {
        public QueryPropertyDetailsToApply(string propertyId)
        {
            PropertyId = propertyId;
        }
        public string PropertyId { get; }
    }
}

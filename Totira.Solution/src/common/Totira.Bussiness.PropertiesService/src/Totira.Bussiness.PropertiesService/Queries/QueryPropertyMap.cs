using Totira.Support.Application.Queries;

namespace Totira.Bussiness.PropertiesService.Queries;

public class QueryPropertyMap : QueryProperty, IQuery
{
    public QueryPropertyMap(
        EnumPropertySortBy? sortBy,
        int pageNumber,
        int pageSize,
        QueryFilter? queryFilter)
        : base(sortBy, pageNumber, pageSize, queryFilter)
    {
    }
}

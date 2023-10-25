using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Totira.Support.Persistance.Util;

namespace Totira.Support.Persistance.Mongo.Util;

public class MongoFilter<T> : IMongoFilter<T>
{
    private FilterDefinition<T> _filterDefinition;

    public MongoFilter()
    {
        _filterDefinition = Builders<T>.Filter.Empty;
    }

    /// <inheritdoc />
    public void AddCondition(Expression<Func<T, bool>> condition) => _filterDefinition &= Builders<T>.Filter.Where(condition);
    /// <inheritdoc />
    public void AddAnyIn<TItem>(Expression<Func<T, IEnumerable<TItem>>> field, IEnumerable<TItem> values) => _filterDefinition &= Builders<T>.Filter.AnyIn(field, values);
    public void AddFilter(IMongoFilter<T> filter) => _filterDefinition &= ((MongoFilter<T>)filter).GetFilterDefinition();
    public void SetFilter(FilterDefinition<T> filter) => _filterDefinition = filter;

    public FilterDefinition<T> GetFilterDefinition() => _filterDefinition;
    
    /// <inheritdoc />
    public bool HasAnyExpr()
    {
        string filterJson = _filterDefinition.Render(
            BsonSerializer.SerializerRegistry.GetSerializer<T>(),
            BsonSerializer.SerializerRegistry).ToString();
        return filterJson.Contains("$expr");
    }
}
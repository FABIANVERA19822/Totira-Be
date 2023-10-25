using System.Linq.Expressions;

namespace Totira.Support.Persistance.Util;

public interface IMongoFilter<T>
{
    /// <summary>
    /// Generic custom filter for add any condition
    /// </summary>
    /// <param name="condition">Condition expression</param>
    void AddCondition(Expression<Func<T, bool>> condition);
    
    /// <summary>
    /// Adds an in filter for an array field.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="field">Document field</param>
    /// <param name="values">Value to compare</param>
    void AddAnyIn<TItem>(Expression<Func<T, IEnumerable<TItem>>> field, IEnumerable<TItem> values);
    
    /// <summary>
    /// Adds a new <see cref="IMongoFilter{T}"/> to this filter
    /// </summary>
    /// <param name="filter">Filter</param>
    void AddFilter(IMongoFilter<T> filter);
    
    /// <summary>
    /// Checks if any of the filters contains any $expr sentence
    /// </summary>
    /// <returns>True or false</returns>
    bool HasAnyExpr();
}
namespace Totira.Support.Application.Queries
{
    public interface IQueryHandler<TRequest, TResponse> where TRequest : IQuery
    {
        Task<TResponse> HandleAsync(TRequest query);
    }
}

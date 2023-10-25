namespace Totira.Support.Api.Connection
{
    public interface IQueryRestClient
    {
        Task<QueryResponse<T>> GetAsync<T>(string url);
    }
}

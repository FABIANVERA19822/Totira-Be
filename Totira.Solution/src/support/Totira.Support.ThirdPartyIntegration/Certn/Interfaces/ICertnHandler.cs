namespace Totira.Business.Integration.Certn.Interfaces
{
    public interface ICertnHandler
    {
        Task<T> GetAsync<T>(string endpoint, object uriParameters, bool retry = true);
        Task<string> GetAsync(string endpoint, object uriParameters, bool retry = true);
        Task<T> PostAsync<T>(string endpoint, object uriParameters, bool retry = true);
        Task<string> PostAsync(string endpoint, object uriParameters, bool retry = true);
    }
}

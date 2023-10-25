using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using Totira.Support.Resilience;

namespace Totira.Support.Api.Connection
{
    public class QueryRestClient : IQueryRestClient
    {
        private readonly IPolicyFactory _policyFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<QueryRestClient> _logger;
        private readonly QueryRestClientOptions _options;

        public QueryRestClient(
            IPolicyFactory policyFactory,
            IHttpClientFactory httpClientFactory,
            ILogger<QueryRestClient> logger,
            IOptions<QueryRestClientOptions> options)
        {
            _policyFactory = policyFactory;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;

        }
        public async Task<QueryResponse<T>> GetAsync<T>(string url)  
        {
            try
            {
                var queryRetries = _options.QueryRetries == 0 ? 1 : _options.QueryRetries;
                var timeOut = _options.TimeOut == 0 ? 5 : _options.TimeOut;

                IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(queryRetries);
                using var response = await policy.ExecuteAsync(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("Accept", "application/json");

                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

                    _logger.LogDebug($"RestClient calling {url}");
                    return await httpClient.SendAsync(request);
                });

                _logger.LogDebug($"Response status code is {response.StatusCode}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    T content = await DeserizalizeResponse<T>(response);
                    return new QueryResponse<T>(response.StatusCode, content);
                }
                else
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    
                    return new QueryResponse<T>(response.StatusCode,contents);
                }
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError($"Timeout calling server at {url}");
                throw new Exceptions.TimeoutException($"Timeout calling server at {url}", ex);

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling server: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing JSON into type {typeof(T).Name}:{ex.Message}");
                throw new Exceptions.DeserializationException($"Error deserializing JSON into type {typeof(T).Name}:{ex.Message}", ex);
            }
        }

        private async Task<T> DeserizalizeResponse<T>(HttpResponseMessage response)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(responseStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            _logger.LogDebug($"Deserialing response content into type {typeof(T).Name}");
            T content = serializer.Deserialize<T>(jsonReader);
            return content;
        }
    }
}

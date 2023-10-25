using Flurl;
using Flurl.Http;
using Totira.Business.Integration.Certn.Interfaces;
using Totira.Business.Integration.Certn.Configurations;
using Totira.Business.Integration.Certn.Extensions;
using Microsoft.Extensions.Options;
using System.Net;

namespace Totira.Business.Integration.Certn.CommonlHandlers
{
    public class CertnHandler : ICertnHandler
    {
        private readonly IOptions<CertnSettings> _configuration;
        public CertnHandler(IOptions<CertnSettings> configuration)
        {
            _configuration = configuration;
        }

        public async Task<T> GetAsync<T>(string endpoint, object uriParameters, bool retry = true)
        {
            var result = await _configuration.Value.CertnBaseUrl
                .AppendPathSegment(endpoint)
                .CertnSetQueryParams(uriParameters)
                .SetBaseRequest()
                .AddSecurityTokenHeader(_configuration.Value.CertnToken)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .CertnGetAsync<T>()
                .ConfigureAwait(false);

            return result;
        }

        public async Task<string> GetAsync(string endpoint, object uriParameters, bool retry = true)
        {
            var result = await _configuration.Value.CertnBaseUrl
                .AppendPathSegment(endpoint)
                .CertnSetQueryParams(uriParameters)
                .SetBaseRequest()
                .AddSecurityTokenHeader(_configuration.Value.CertnToken)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .CertnGetAsync()
                .ConfigureAwait(false);

            return result;
        }

        public async Task<T> PostAsync<T>(string endpoint, object bodyParameters, bool retry = true)
        {
            var result = await _configuration.Value.CertnBaseUrl
                .AppendPathSegment(endpoint)
                .SetBaseRequest()
                .AddSecurityTokenHeader(_configuration.Value.CertnToken)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .CertnPostAsync<T>(bodyParameters)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<string> PostAsync(string endpoint, object body, bool retry = true)
        {
            var result = await _configuration.Value.CertnBaseUrl
                .AppendPathSegment(endpoint)
                .SetBaseRequest()
                .AddSecurityTokenHeader(_configuration.Value.CertnToken)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .AllowHttpStatus(HttpStatusCode.OK)
                .AllowHttpStatus(HttpStatusCode.Created)
                .CertnPostAsync(body)
                .ConfigureAwait(false);

            return result;
        }
    }
}

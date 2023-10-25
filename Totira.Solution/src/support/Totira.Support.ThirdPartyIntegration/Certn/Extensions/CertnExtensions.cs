using Flurl.Http;
using Flurl;
using System.Net;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Totira.Business.Integration.Certn.Extensions
{
    public static class CertnExtensions
    {
        private const string UnknownErrorMessage = "There was an error in the call and the service did not return an error message";
        private const string ExceedsTheLimitsResponseMessage = "Exceeds the limits";

        public static IFlurlRequest SetBaseRequest(this Url requestUrl)
        {
            return requestUrl                
                .WithHeader("Accept", "application/json");
        }

        public static IFlurlRequest AddSecurityTokenHeader(this IFlurlRequest request, string token)
        {
            return request.WithOAuthBearerToken(token);
        }

        public static async Task<T> CertnPostAsync<T>(this IFlurlRequest request, object bodyParameters)
        {
            HttpResponseMessage response = (HttpResponseMessage)await request.PostJsonAsync(bodyParameters).ConfigureAwait(false);
            var plainResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var okResponses = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created };

            if (!okResponses.Contains(response.StatusCode))
                return DetermineErrorReasonWhenBadHttpStatusCode<T>(response);
            
            return JsonConvert.DeserializeObject<T>(plainResponse) ?? default!;
        }

        public static async Task<string> CertnPostAsync(this IFlurlRequest request, object body)
        {
            try
            {              
                
                var response = await request.PostJsonAsync(body);   
                var plainText = response.ResponseMessage;
                var okResponses = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created };
                if (!okResponses.Contains((HttpStatusCode)response.StatusCode))
                    return DetermineErrorReasonWhenBadHttpStatusCode<string>(response.ResponseMessage);

                var result = await response.GetJsonAsync();
                return await plainText.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
         }

        public static async Task<T> CertnGetAsync<T>(this IFlurlRequest request)
        {
            HttpResponseMessage response = (HttpResponseMessage)await request.GetAsync().ConfigureAwait(false);
            var plainResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return DetermineErrorReasonWhenBadHttpStatusCode<T>(response);
            }

            if (!IsCerntErrorResponse(plainResponse))
            {
                return JsonConvert.DeserializeObject<T>(plainResponse);
            }

            return DetermineErrorReasonWhenBadHttpStatusCode<T>(response);
        }

        public static async Task<string> CertnGetAsync(this IFlurlRequest request)
        {
            try
            {

                var response = await request.GetAsync();
                var plainText = response.ResponseMessage;
                var okResponses = new List<HttpStatusCode> { HttpStatusCode.OK };
                if (!okResponses.Contains((HttpStatusCode)response.StatusCode))
                    return DetermineErrorReasonWhenBadHttpStatusCode<string>(response.ResponseMessage);

                var result = response.GetJsonAsync();
                return await plainText.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static T DetermineErrorReasonWhenBadHttpStatusCode<T>(HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => default!,
                HttpStatusCode.Unauthorized => throw DetermineUnauthorizedReason(response),
                _ => throw new Exception(response.StatusCode.ToString())
            };
        }

        private static Exception DetermineUnauthorizedReason(HttpResponseMessage response)
        {
            if (IsExceedsTheLimitsResponse(response.ReasonPhrase))
            {
                return new Exception(response.ReasonPhrase);
            }
            return new AuthenticationException();
        }

        private static bool IsExceedsTheLimitsResponse(string errorResponse)
        {
            return (errorResponse).IndexOf("Exceeds the limits", StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private static bool IsCerntErrorResponse(string jsonResponse)
        {
            return (jsonResponse).IndexOf("errorid", StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static Url CertnSetQueryParams(this Url request, object filterParams)
        {
            var filterObjectType = filterParams.GetType();
            var filterObjectTypePropertyInfo = filterObjectType.GetProperties();
            var filters = filterObjectTypePropertyInfo
                .Where(filter => filter.GetValue(filterParams) != null)
                .Select(p => EncodeParameter(p.Name, p.GetValue(filterParams)?.ToString() ?? string.Empty)).ToList();
            var hasFilters = false;
            var uri = new StringBuilder();

            if (filters.Count > 0)
            {
                hasFilters = true;
                uri.Append("?$filter=");
                uri.AppendJoin(" and ", filters);
                filters.Clear();
            }

            if (filterObjectTypePropertyInfo.Any(p => p.Name == "OrderBy"))
            {
                var orderByProperty = filterObjectTypePropertyInfo.Single(p => p.Name == "OrderBy");
                if (orderByProperty.GetValue(filterParams) != null)
                {
                    filters.Add($"$orderby={orderByProperty.GetValue(filterParams)}");
                }
            }

            if (filters.Count == 0)
            {
                //return request.Path + uri.ToString().Replace("%3a", ":");
                return $"{request.Root}{request.Path}";
            }

            uri.Append(hasFilters ? "&" : "?");
            uri.AppendJoin("&", filters);

            return request.Path + uri.ToString().Replace("%3a", ":");
        }

        private static string EncodeParameter(string parameterName, string parameterValue)
        {
            return string.Format("{1}%20eq%20'{0}'", EncodeQueryParamValue(parameterValue), parameterName);
        }

        private static string EncodeQueryParamValue(string paramValue)
        {
            return !string.IsNullOrWhiteSpace(paramValue) ? HttpUtility.UrlEncode(paramValue) : string.Empty;
        }
    }
}

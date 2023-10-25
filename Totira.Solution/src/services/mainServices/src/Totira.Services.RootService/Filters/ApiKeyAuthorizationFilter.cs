using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Totira.Services.RootService.Options;

namespace Totira.Services.RootService.Filters
{
    public class ApiKeyAuthorizationFilter : IAsyncActionFilter
    {
        private readonly ILogger<ApiKeyAuthorizationFilter> _logger;
        private readonly ApiSecurityOptions _apiSecurityOptions;

        public ApiKeyAuthorizationFilter(ILogger<ApiKeyAuthorizationFilter> logger, ApiSecurityOptions apiSecurityOptions)
        {
            _logger = logger;
            _apiSecurityOptions = apiSecurityOptions;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiKey = GetValueByKey(context.HttpContext, "apikey");
            var inboundSystem = GetValueByKey(context.HttpContext, "inboundsystem").ToUpper();
            var internalApiKey = string.Empty;
                        
            if (!string.IsNullOrEmpty(inboundSystem))
            {
                switch (inboundSystem)
                {
                    case "JIRA":
                        internalApiKey = _apiSecurityOptions.ApiKey_JIRA; break;
                    case "PERSONA":
                        internalApiKey = _apiSecurityOptions.ApiKey_PERSONA; break;
                    default:
                        break;
                }
            }

            if (internalApiKey.Equals(apiKey))
            {
                await next();
            }
            else
            {
                var message = "The Token is missing.";
                _logger.LogError(message);
                throw new UnauthorizedAccessException(message);
            }
        }

        private string GetValueByKey(HttpContext context, string key)
        {
            var value = string.Empty;
            value = context.Request.Headers[key].FirstOrDefault();

            if (string.IsNullOrEmpty(value))
            {
                value = context.Request.Query[key].ToString();
            }

            return value;
        }
    }
}

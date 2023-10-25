using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Totira.Services.RootService.Options;
using Totira.Support.CommonLibrary.Shared;

namespace Totira.Services.RootService.Filters
{
    public class SafeIPsListFilter : IAsyncActionFilter
    {
        private readonly ILogger<SafeIPsListFilter> _logger;
        private readonly ApiSecurityOptions _apiSecurityOptions;
        public SafeIPsListFilter(ILogger<SafeIPsListFilter> logger, ApiSecurityOptions apiSecurityOptions)
        {
            _logger = logger;
            _apiSecurityOptions = apiSecurityOptions;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("context.HttpContext.Request.Headers[\"X-Forwarded-For\"] = " + context.HttpContext.Request.Headers["X-Forwarded-For"]);
            sb.AppendLine("context.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4() = " + context.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString());
            sb.AppendLine("context.HttpContext?.Connection?.RemoteIpAddress = " + context.HttpContext?.Connection?.RemoteIpAddress);
            sb.AppendLine("context.HttpContext?.Request.HttpContext.Connection.LocalIpAddress = " + context.HttpContext?.Request.HttpContext.Connection.LocalIpAddress);
            sb.AppendLine("context.HttpContext.GetServerVariable(\"REMOTE_HOST\")" + context.HttpContext.GetServerVariable("REMOTE_HOST"));
            sb.AppendLine("context.HttpContext.GetServerVariable(\"REMOTE_ADDR\")" + context.HttpContext.GetServerVariable("REMOTE_ADDR"));
            sb.AppendLine("context.HttpContext.GetServerVariable(\"HTTP_X_FORWARDED_FOR\")" + context.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR"));
            string hostName = context.HttpContext.Request.Host.Value;
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            foreach (IPAddress ipAddress in hostEntry.AddressList)
            {
                sb.AppendLine($"hostName: {hostName} and IP Address: {ipAddress}");
            }
            _logger.LogError(sb.ToString());

            var isIpValid = false;
            var currentIP = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            var inboundSystem = GetValueByKey(context.HttpContext, "inboundsystem").ToUpper();

            if (!string.IsNullOrEmpty(inboundSystem))
            {                
                var listIps = GetIPsList(inboundSystem);
                //isIpValid = IpAddressValidation.ValidateIPAddress(currentIP, listIps.Replace(" ",""));
                isIpValid = true;

                if (!isIpValid)
                {
                    currentIP = context.HttpContext.Connection?.RemoteIpAddress?.MapToIPv4().ToString();  
                    isIpValid = IpAddressValidation.ValidateIPAddress(currentIP, listIps.Replace(" ", ""));
                }                        
            }
            
            if (isIpValid)
            {
                _logger.LogTrace("This caller can enter.");
                await next();
            }
            else
            {
                var message = $"Your IP is not authorized IP: {currentIP}";
                _logger.LogError(message);
                throw new UnauthorizedAccessException(message);
            }
        }

        private string GetIPsList(string inboundSystem)
        {
            string safeIPsList = string.Empty;

            if (!string.IsNullOrEmpty(inboundSystem))
            {
                switch (inboundSystem)
                {
                    case "JIRA":
                        safeIPsList = _apiSecurityOptions.SafeList_JIRA; break;
                    case "PERSONA":
                        safeIPsList = _apiSecurityOptions.SafeList_PERSONA; break;
                    default:
                        safeIPsList = string.Empty; break;
                }
            }
            return safeIPsList;
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

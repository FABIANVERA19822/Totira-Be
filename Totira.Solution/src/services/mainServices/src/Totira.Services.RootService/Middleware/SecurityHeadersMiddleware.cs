using Microsoft.Extensions.Options;
using Totira.Services.RootService.DTO.Security;

namespace Totira.Services.RootService.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersConfiguration _headersConfig;

        public SecurityHeadersMiddleware(RequestDelegate next, IOptions<SecurityHeadersConfiguration> headersConfig)
        {
            _next = next;
            _headersConfig = headersConfig.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["Strict-Transport-Security"] = _headersConfig.StrictTransportSecurity;
                context.Response.Headers["X-Frame-Options"] = _headersConfig.XFrameOptions;
                context.Response.Headers["X-Content-Type-Options"] = _headersConfig.XContentTypeOptions;
                context.Response.Headers["Content-Security-Policy"] = _headersConfig.ContentSecurityPolicy;
                context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = _headersConfig.XPermittedCrossDomainPolicies;
                context.Response.Headers["Referrer-Policy"] = _headersConfig.ReferrerPolicy;
                context.Response.Headers["Clear-Site-Data"] = _headersConfig.ClearSiteData;
                context.Response.Headers["Cross-Origin-Embedder-Policy"] = _headersConfig.CrossOriginEmbedderPolicy;
                context.Response.Headers["Cross-Origin-Opener-Policy"] = _headersConfig.CrossOriginOpenerPolicy;
                context.Response.Headers["Cross-Origin-Resource-Policy"] = _headersConfig.CrossOriginResourcePolicy;
                context.Response.Headers["Cache-Control"] = _headersConfig.CacheControl;

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }

}

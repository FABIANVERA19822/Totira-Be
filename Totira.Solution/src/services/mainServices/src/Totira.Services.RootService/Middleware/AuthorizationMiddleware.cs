using System.IdentityModel.Tokens.Jwt;

namespace Totira.Services.RootService.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var authorization = string.Empty;
            var userName = string.Empty;
            if (context.Request.Headers != null && !string.IsNullOrEmpty(context.Request.Headers.Authorization))
            {
                authorization = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(authorization);
                userName = token.Claims?.FirstOrDefault(claim => claim.Type == "cognito:username")?.Value ?? token.Claims?.FirstOrDefault(claim => claim.Type == "username")?.Value;
                context.Items["tenantId"] = userName;
            }
            await _next(context);
        }
    }
}

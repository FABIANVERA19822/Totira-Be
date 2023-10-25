using Microsoft.Extensions.Options;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Configurations;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Totira.Support.CommonLibrary.CommonlHandlers
{
    public class SecurityHandler : ISecurityHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly CognitoOptions _cognitoOptions;
        private readonly ILogger<SecurityHandler> _logger;

        public SecurityHandler(IOptions<CognitoOptions> cognitoOptions,
            ILogger<SecurityHandler> logger)
        {
            _cognitoOptions = cognitoOptions.Value;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("client_secret", _cognitoOptions.Token.ClientSecret ),
                    new KeyValuePair<string, string>("scope",_cognitoOptions.Token.Scope ),
                    new KeyValuePair<string, string>("grant_type", _cognitoOptions.Token.GrantType ),
                    new KeyValuePair<string, string>("client_id",  _cognitoOptions.Token.ClientId),
                });
                // "https://totirafe51658678-51658678-dev.auth.us-east-1.amazoncognito.com/oauth2/token"
                using HttpResponseMessage response = await client.PostAsync(_cognitoOptions.Token.Url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var cognitoToken = JsonSerializer.Deserialize<CognitoToken>(responseBody) ?? new CognitoToken();

                return $"{cognitoToken.TokenType} {cognitoToken.AccessToken}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "It wasn't possible. Obtain cognito accessToken");
                throw;
            }
        }
    }
}
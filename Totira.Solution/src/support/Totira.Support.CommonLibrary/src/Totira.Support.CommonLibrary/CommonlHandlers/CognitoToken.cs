using System.Text.Json.Serialization;

namespace Totira.Support.CommonLibrary.CommonlHandlers
{
    public class CognitoToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }
}
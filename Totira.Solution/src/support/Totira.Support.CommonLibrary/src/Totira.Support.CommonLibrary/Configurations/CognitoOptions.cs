namespace Totira.Support.CommonLibrary.Configurations
{
    public class CognitoOptions
    {
        public string ProjectId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public TokenInfo Token { get; set; } = new TokenInfo();
    }

    public class TokenInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
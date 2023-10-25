using MongoDB.Driver.Core.Events;

namespace Totira.Support.Otp.Configurations
{
    public class OtpOptions
    {
        public int Retries { get; set; } = 3;
        public int ExpirationTime { get; set; } = 3600;
        public int AmountUse { get; set; } = 1;

        public List<ScopeItem> Scopes { get; set; } = new List<ScopeItem> { };

        public class ScopeItem
        {
            public string Id { get; set; } = string.Empty;
            public double Expiration { get; set; }
        }
    }

}
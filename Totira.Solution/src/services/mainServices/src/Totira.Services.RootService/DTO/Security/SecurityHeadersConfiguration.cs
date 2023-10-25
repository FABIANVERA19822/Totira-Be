using Microsoft.Extensions.Options;

namespace Totira.Services.RootService.DTO.Security
{
    public class SecurityHeadersConfiguration
    {
        public string StrictTransportSecurity { get; set; }
        public string XFrameOptions { get; set; }
        public string XContentTypeOptions { get; set; }
        public string ContentSecurityPolicy { get; set; }
        public string XPermittedCrossDomainPolicies { get; set; }
        public string ReferrerPolicy { get; set; }
        public string ClearSiteData { get; set; }
        public string CrossOriginEmbedderPolicy { get; set; }
        public string CrossOriginOpenerPolicy { get; set; }
        public string CrossOriginResourcePolicy { get; set; }
        public string CacheControl { get; set; }
    }
}

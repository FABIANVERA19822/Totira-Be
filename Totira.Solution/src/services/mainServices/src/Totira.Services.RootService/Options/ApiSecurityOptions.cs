using Microsoft.Extensions.Options;

namespace Totira.Services.RootService.Options
{
    public class ApiSecurityOptions
    {
        public string ApiKey_JIRA { get; set; } = string.Empty;
        public string ApiKey_PERSONA { get; set; } = string.Empty;
        public string SafeList_JIRA { get; set; } = string.Empty;
        public string SafeList_PERSONA { get; set; } = string.Empty;
    }
}
namespace Totira.Business.ThirdPartyIntegrationService.Options
{
    public class JiraOptions
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string User { get; set; }
        public string StatusComplete { get; set; }
        public string StatusRejected { get; set; }
        public string ProjectId { get; set; }
        public string IssueTypeId { get; set; }
        public string AgentProjectId { get; set; }
        public string AgentIssueTypeId { get; set; }

    }
}


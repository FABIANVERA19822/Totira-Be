namespace Totira.Business.ThirdPartyIntegrationService.Helpers.Jira
{
    public class JiraTicket
    {
        public Fields fields { get; set; }
        public Update update { get; set; }
    }

    public class Fields
    {
        public Description description { get; set; }
        public string duedate { get; set; }
        public Issuetype issuetype { get; set; }
        public string[] labels { get; set; }
        public Project project { get; set; }
        public string summary { get; set; }
        public string customfield_10060 { get; set; }
    }

    public class Description
    {
        public Contents[] content { get; set; }
        public string type { get; set; }
        public int version { get; set; }
    }

    public class Contents
    {
        public Contents[] content { get; set; }
        public string type { get; set; }
        public string text { get; set; }
    }

    public class Issuetype
    {
        public int id { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
    }

    public class Update
    {
    }

}

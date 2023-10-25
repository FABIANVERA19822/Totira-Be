using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Totira.Business.ThirdPartyIntegrationService.Options;

namespace Totira.Business.ThirdPartyIntegrationService.Helpers.Jira
{
    public static class JiraTicketHelper
    {

        /// <summary>
        /// this build a base object to send a new issue for jira
        /// </summary>
        /// <param name="issueType"></param>
        /// <param name="title"></param>
        /// <param name="projectId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static JiraTicket BuildBaseTicket(int issueType, string title, string projectId, string description, string[] labels = null)
        {
            var jiraTicket = new JiraTicket();

            jiraTicket.fields = new Fields();
            jiraTicket.fields.issuetype = new Issuetype();
            jiraTicket.fields.issuetype.id = issueType;
            jiraTicket.fields.project = new Project();
            jiraTicket.fields.project.id = projectId;
            jiraTicket.fields.summary = title;
            jiraTicket.fields.description = new Description();
            jiraTicket.fields.description.version = 1;
            jiraTicket.fields.description.type = "doc";
            if (labels != null)
            {
                jiraTicket.fields.labels = labels;
            }
            

            return jiraTicket;
        }

        public static Contents BuildParagraph(string text) {            
            List<Contents> paragraphText = new List<Contents>();            
            paragraphText.Add(new Contents() { type = "text", text = text });
            return new Contents() { type = "paragraph", content = paragraphText.ToArray() };
        }

       

        public static Contents BuildBulletPoints(List<string> texts) {
            Contents bulletPoints = new Contents();
            bulletPoints.type = "bulletList";

            List<Contents> itemsList = new List<Contents>();
            foreach (string text in texts)
            {
                List<Contents> paragraphBulletPoint = new List<Contents>();
                paragraphBulletPoint.Add(BuildParagraph(text));
                itemsList.Add(new Contents() { type = "listItem", content =paragraphBulletPoint.ToArray() });
            }

            bulletPoints.content = itemsList.ToArray();
            return bulletPoints;
        }



        /// <summary>
        /// Send a request for create a new ticket on jira
        /// </summary>
        /// <param name="userName">user asociated to the api key</param>
        /// <param name="jiraApiKey">the api key of jira</param>
        /// <param name="jiraUrl">url of the project</param>
        /// <param name="baseTicket">base object ticket</param>
        /// <returns>jira obj result</returns>
        public static async Task<CreationJiraResponse> SendTicketToJira(JiraOptions options, JiraTicket baseTicket)
        {

            HttpClient Client = new HttpClient();
            var authData = System.Text.Encoding.UTF8.GetBytes($"{options.User}:{options.ApiKey}");
            var basicAuthentication = Convert.ToBase64String(authData);
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthentication);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.BaseAddress = new Uri(options.Url);

            string jsonIgnoreNullValues = JsonConvert.SerializeObject(baseTicket, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var response = await Client.PostAsync("/rest/api/3/issue", new StringContent(jsonIgnoreNullValues, Encoding.UTF8, "application/json"));
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                throw new Exception("Error on creation ticket in jira");

            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var objResponse = JsonConvert.DeserializeObject<CreationJiraResponse>(json);

            return objResponse;

        }

        public static async Task AttachFileToJira(JiraOptions options, string issueId, string fileName, string extension, byte[] fileContents)
        {


            MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
            ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
            byteArrayContent.Headers.Add("Contents-Type", extension);
            byteArrayContent.Headers.Add("X-Atlassian-Token", "no-check");
            multiPartContent.Add(byteArrayContent, "file", fileName);


            HttpClient Client = new HttpClient();
            var authData = System.Text.Encoding.UTF8.GetBytes($"{options.User}:{options.ApiKey}");
            var basicAuthentication = Convert.ToBase64String(authData);
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthentication);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
            Client.BaseAddress = new Uri(options.Url);
            try
            {
                var httpResponse = await Client.PostAsync($"/rest/api/3/issue/{issueId}/attachments", multiPartContent);
                HttpStatusCode statusCode = httpResponse.StatusCode;
                HttpContent responseContent = httpResponse.Content;

                if (responseContent != null)
                {
                    string stringContentsTask = await responseContent.ReadAsStringAsync();
                    //string stringContents = stringContentsTask.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class CreationJiraResponse
        {
            public string id { get; set; }
            public string key { get; set; }
            public string self { get; set; }
        }
    }
}

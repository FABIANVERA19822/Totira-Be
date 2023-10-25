
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Support.CommonLibrary.CommonlHandlers
{
    public class EmailHandler : IEmailHandler
    {
        private readonly IOptions<SesSettings> _configuration;
        private readonly ILogger<EmailHandler> _logger;
        public EmailHandler(
            IOptions<SesSettings> configuration,
            ILogger<EmailHandler> logger

            )
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> SendBulkEmailAsync(List<string> listEmails, string subject, string emailBody)
        {
            bool result = false;
            var request = BuildBaseRequest(listEmails, subject, emailBody);
            using (var client = new AmazonSimpleEmailServiceClient(_configuration.Value.AwsEmailAccessKey, _configuration.Value.AwsEmailAccessSecretKey))
            {
                var response = await client.SendEmailAsync(request);
                result = response.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
            }
            return result;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string emailBody)
        {
            bool result = false;

            if (!_configuration.Value.SendEmails)
            {
                return true;
            }

            try
            {
                var request = BuildBaseRequest(new List<string>() { email }, subject, emailBody);

                using (var client = new AmazonSimpleEmailServiceClient(_configuration.Value.AwsEmailAccessKey, _configuration.Value.AwsEmailAccessSecretKey, Amazon.RegionEndpoint.USEast1))
                {
                    var response = await client.SendEmailAsync(request);
                    result = response.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"Error sending email to :{email}");
            }

            return result;
        }

        private SendEmailRequest BuildBaseRequest(List<string> emails, string subject, string emailBody)
        {
            var request = new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = emails
                },
                Message = new Message
                {
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = emailBody
                        }
                    },
                    Subject = new Content
                    {
                        Charset = "UTF-8",
                        Data = subject
                    }
                },
                Source = _configuration.Value.AwsSourceEmail
            };

            return request;
        }
    }
}

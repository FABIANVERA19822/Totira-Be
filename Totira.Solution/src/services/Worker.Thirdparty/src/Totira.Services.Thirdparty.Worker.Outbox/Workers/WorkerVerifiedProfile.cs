using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.EmailTemplates;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using ListTenantVerifiedProfileDto = Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO.ListTenantVerifiedProfileDto;

namespace Totira.Services.Thirdparty.Worker.Outbox.Workers
{
    public class WorkerVerifiedProfile : BackgroundService
    {
        private readonly ILogger<WorkerVerifiedProfile> _logger;
        private readonly WorkerVerifiedProfileOptions _options;
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;

        public WorkerVerifiedProfile(
            ILogger<WorkerVerifiedProfile> logger,
            IOptions<WorkerVerifiedProfileOptions> options,
            IOptions<RestClientOptions> restClientOptions,
            IQueryRestClient queryRestClient,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings)
        {
            _logger = logger;
            _options = options.Value;
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
            _emailHandler = emailHandler;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string subject = "Your profile is ready to review";
            string emailBody = EmailTemplateResource.VerifiedProfile;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Verified Profile Worker running at: {time}", DateTimeOffset.Now);

                    var response = await _queryRestClient.GetAsync<ListTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/");
                    var result = response.Content;

                    if (result.Count > 0)
                    {
                        foreach (var process in result.VerifiedProfiles)
                        {
                            _logger.LogDebug($"Starting to process message with id {process.Id}");
                            // Obtain Tenant Contact Information
                            var getEmail = await GetEmailForContactInformation(process.TenantId);
                            var tenantId = process.TenantId;
                            if (getEmail != null && getEmail != $"Missing contact information for Tenant id {tenantId}")
                            { 
                                // Send email confirmation for verified profile
                                string emailTo = getEmail;
                                var isSent = await SendEmailConfirmation(emailTo, subject, emailBody, process.TenantId);
                                if (isSent)
                                {
                                    var verification = (await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/EmailConfirmation/{process.TenantId}"));
                                    var result1 = verification.Content;
                                    _logger.LogWarning($"Email confirmation was sent for Tenant id {process.TenantId}");
                                }
                            }
                            _logger.LogDebug($"Finished processing message with id {process.Id}");

                            // Tenant Group Application process
                            // Obtain Application Request Information
                            var urlAppReq = $"{_restClientOptions.User}/ApplicationRequest/{process.TenantId}";
                            var infoAppReq = await _queryRestClient.GetAsync<GetTenantApplicationRequestDto>(urlAppReq);
                            var resultAppReq = infoAppReq.Content;
                            if (resultAppReq.IsMulti == true)
                            {
                                // Send email for Coapplicants
                                foreach (var coapplicant in resultAppReq.Coapplicants) 
                                {
                                    if (coapplicant.Email != null && coapplicant.Email != getEmail)
                                    {
                                        var isSent = await SendEmailConfirmation(coapplicant.Email, subject, emailBody, (Guid)coapplicant.CoapplicantId);
                                        if (isSent)
                                        {
                                            var verification = (await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/EmailConfirmation/{coapplicant.CoapplicantId}"));
                                            var result1 = verification.Content;
                                            _logger.LogWarning($"Email confirmation was sent for Tenant id {coapplicant.CoapplicantId}");
                                        }
                                    }
                                }
                                // Send email for Guarantor
                                if (resultAppReq.Guarantor.Email != null && resultAppReq.Guarantor.Email != getEmail)
                                {
                                    var guarantorEmail = resultAppReq.Guarantor.Email;
                                    var isSent = await SendEmailConfirmation(guarantorEmail, subject, emailBody, (Guid)resultAppReq.Guarantor.CoapplicantId);
                                    if (isSent)
                                    {
                                        var verification = (await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/EmailConfirmation/{resultAppReq.Guarantor.CoapplicantId}"));
                                        var result1 = verification.Content;
                                        _logger.LogWarning($"Email confirmation was sent for Tenant id {resultAppReq.Guarantor.CoapplicantId}");
                                    }
                                }
                            }
                        }
                    }
                    
                    _logger.LogDebug("Verified Profile Worker process of Messages is finished at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error on Verified Profile Worker ProcessPending: {ex.Message}", ex);
                }
                await Task.Delay(TimeSpan.FromMinutes(_options.Interval), stoppingToken);
            }
        }

        private async Task<string> GetEmailForContactInformation(Guid tenantId)
        {
            string email ="";
            // Obtain Tenant Contact Information
            var url = $"{_restClientOptions.User}/user/tenant/contactinfo/{tenantId}";
            var info = await _queryRestClient.GetAsync<GetTenantContactInformationDto>(url);

            if (info.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var Message = info.ErrorMessage.Substring(1);
                Message = Message.Substring(0, Message.Length - 1);
                _logger.LogWarning($"Missing contact information for Tenant id {tenantId}");
                email = $"Missing contact information for Tenant id {tenantId}";
            } else
            {
                email = info.Content.Email;
            }
            return email;
        }

        private async Task<bool> SendEmailConfirmation(string emailTo, string subject, string emailBody, Guid id)
        {
            // Now we have to send the email confirmation for the verified profile

            var link = EmailHelper.BuildVerifiedTenantProfileLink(
                baseUrl: _settings.Value.Url,
                tenantId: id);

            var FormatEmailBody = emailBody.Replace("[Link]", link);

            var isSent = await _emailHandler.SendEmailAsync(emailTo, subject, FormatEmailBody);
            if (!isSent)
                _logger.LogError("Fail sending email.");

            return isSent;
        }
    }
}

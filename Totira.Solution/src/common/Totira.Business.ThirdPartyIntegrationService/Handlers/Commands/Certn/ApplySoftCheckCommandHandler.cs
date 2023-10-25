using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Totira.Business.Integration.Certn.Interfaces;
using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Certn;

public class ApplySoftCheckCommandHandler : IMessageHandler<ApplySoftCheckCommand>
{
    private readonly ICertnHandler _certnHandler;
    private readonly IRepository<TenantApplications, string> _tenantApplicationsRepository;
    private readonly ILogger<ApplySoftCheckCommandHandler> _logger;
    private static string  messageBadRequest = "Bad Request";

    public ApplySoftCheckCommandHandler(
        ICertnHandler certnHandler,
        IRepository<TenantApplications, string> tenantApplicationsRepository,
        ILogger<ApplySoftCheckCommandHandler> logger)
    {
        _certnHandler = certnHandler;
        _tenantApplicationsRepository = tenantApplicationsRepository;
        _logger = logger;
    }

    public async Task HandleAsync(IContext context, ApplySoftCheckCommand message)
    {
        var tenant = await _tenantApplicationsRepository.GetByIdAsync(message.TenantId.ToString()) ??
            TenantApplications.Empty(message.TenantId.ToString());

        if (tenant.Applications.Any())
        {
            _logger.LogWarning("Tenant has already a Certn validation process in progress.");
            return;
        }

        var response = await _certnHandler.PostAsync("applications/quick/", message.Request);

        if (response != null && response.Length > 0 && !response.Contains(messageBadRequest, StringComparison.OrdinalIgnoreCase))
        {
            JObject result = JObject.Parse(response);

            var applicationId = (string?)result.SelectToken("application.id");
            var applicantId = (string?)result.SelectToken("application.applicants[0].id");
            var statusRisk = (string?)result.SelectToken("application.applicants[0].report_summary.risk_result.status");
            var statusEquifax = (string?)result.SelectToken("application.applicants[0].report_summary.equifax_result.status");

            tenant.Applications.Add(
                TenantApplication.CreateSoftCheck(
                    applicationId,
                    string.IsNullOrEmpty(applicantId) ? Guid.Empty : Guid.Parse(applicantId),
                    statusEquifax,
                    statusRisk,
                    response));

            await _tenantApplicationsRepository.Add(tenant);

        }
        else
        {
            response = messageBadRequest;
        }
    }
}

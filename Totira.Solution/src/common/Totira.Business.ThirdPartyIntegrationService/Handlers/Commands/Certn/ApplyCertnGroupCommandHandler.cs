using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Totira.Business.Integration.Certn.Interfaces;
using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Certn;

public class ApplyCertnGroupCommandHandler : IMessageHandler<ApplyCertnGroupCommand>
{
    private readonly ILogger<ApplyCertnGroupCommandHandler> _logger;
    private readonly ICertnHandler _certnHandler;
    private readonly IRepository<TenantApplications, string> _tenantApplicationsRepository;
    private const string messageBadRequest = "Bad Request";

    public ApplyCertnGroupCommandHandler(
        ILogger<ApplyCertnGroupCommandHandler> logger,
        ICertnHandler certnHandler,
        IRepository<TenantApplications, string> tenantApplicationsRepository)
    {
        _logger = logger;
        _certnHandler = certnHandler;
        _tenantApplicationsRepository = tenantApplicationsRepository;
    }

    public async Task HandleAsync(IContext context, Either<Exception, ApplyCertnGroupCommand> command)
    {
        await command.MatchAsync(async cmd => {
            foreach (var singleTenant in cmd.TenantGroups)
            {
                var tenantCertnApplication = await _tenantApplicationsRepository.GetByIdAsync(singleTenant.TenantId.ToString()) ??
                    TenantApplications.Empty(singleTenant.TenantId.ToString());

                if (tenantCertnApplication.Applications.Any())
                {
                    _logger.LogError("Tenant {tenantId} has already a Certn validation process in progress.", singleTenant.TenantId);
                    continue;
                }

                var response = await _certnHandler.PostAsync("applications/quick/", singleTenant.RequestModel);

                if (response != null && response.Length > 0 && !response.Contains(messageBadRequest, StringComparison.OrdinalIgnoreCase))
                {
                    JObject result = JObject.Parse(response);

                    var applicationId = (string?)result.SelectToken("application.id");
                    var applicantId = (string?)result.SelectToken("application.applicants[0].id");
                    var statusRisk = (string?)result.SelectToken("application.applicants[0].report_summary.risk_result.status");
                    var statusEquifax = (string?)result.SelectToken("application.applicants[0].report_summary.equifax_result.status");

                    tenantCertnApplication.Applications.Add(
                        TenantApplication.CreateSoftCheck(
                            applicationId ?? string.Empty,
                            string.IsNullOrEmpty(applicantId) ? Guid.Empty : Guid.Parse(applicantId),
                            statusEquifax ?? string.Empty,
                            statusRisk ?? string.Empty,
                            response));

                    await _tenantApplicationsRepository.Add(tenantCertnApplication);
                }
            }
        }, ex => {
            _logger.LogError(ex, "Error while processing ApplyCertnGroupCommand");
            throw ex;
        });
    }
}
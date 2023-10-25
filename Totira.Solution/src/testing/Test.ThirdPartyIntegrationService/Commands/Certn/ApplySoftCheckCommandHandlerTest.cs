using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Test.ThirdPartyIntegrationService.RepoMocks.Certn;
using Totira.Business.Integration.Certn.Interfaces;
using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Certn;
using Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;
using static Totira.Support.Persistance.IRepository;

namespace Test.ThirdPartyIntegrationService.Commands.Certn;

public class ApplySoftCheckCommandHandlerTest
{
    private readonly Mock<ICertnHandler> _certnHandler;
    private readonly Mock<IRepository<TenantApplications, string>> _tenantApplicationsRepository;
    private readonly Mock<ILogger<ApplySoftCheckCommandHandler>> _loggerMock;
    public ApplySoftCheckCommandHandlerTest() 
    {
        _certnHandler = new Mock<ICertnHandler>();
        _tenantApplicationsRepository = new Mock<IRepository<TenantApplications, string>>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<ApplySoftCheckCommandHandler>>();
    }
    [Fact]
    public async Task HandleAsyncTest_Ok()
    {
        // Arrange
        var tenantId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var softCheckCommand = new ApplySoftCheckCommand(tenantId,
                new SoftCheckRequestModel
                {
                    RequestSoftCheck = true,
                    Information = new SoftCheckRequestModel.InformationModel()
                    {
                        FirstName = "Andrew",
                        LastName = "McLeod",
                        Addresses = "1006 Fort Street Victoria BC CA"
                            .Select(x => new InformationAddressModel
                            {
                                Address = "1006 Fort Street",
                                City = "Victoria",
                                Country = "CA",
                                Current = true,
                                State = "BC"
                            })
                            .ToList()
                    },
                    PropertyLocation = new()
                    {
                        Address = "1410 Blanshard St",
                        City = "Victoria",
                        County = "",
                        Country = "CA",
                        ProvinceState = "BC",
                        LocationType = "Property Location",
                        PostalCode = "V8W2J2"
                    }
                });
        var tenant = new TenantApplications(tenantId.ToString(), null, DateTimeOffset.Now);
        // Act
        var response = "";

        JObject result = JObject.Parse(response);

        var applicationId = (string?)result.SelectToken("application.id");
        var applicantId = (string?)result.SelectToken("application.applicants[0].id");
        var statusRiskResult = (string?)result.SelectToken("risk_result.status");
        var statusEquifax = (string?)result.SelectToken("equifax_result.status");

        tenant.Applications.Add(
            TenantApplication.CreateSoftCheck(
                applicationId,
                string.IsNullOrEmpty(applicantId) ? Guid.Empty : Guid.Parse(applicantId),
                statusRiskResult,
                statusEquifax,
                response));
    }
}
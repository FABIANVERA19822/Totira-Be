﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantApplicationDetailsByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _tenatApplicationDetailsRepositoryMock;
        private readonly Mock<IRepository<TenantVerificationProfile, Guid>> _tenantVerificationProfileMock;
        private readonly Mock<IQueryRestClient> _queryRestClientMock;
        private readonly Mock<IOptions<RestClientOptions>> _restClientOptionsMock;
        protected readonly Mock<ILogger<GetTenantApplicationDetailsByIdQueryHandler>> _loggerMock;
        public GetTenantApplicationDetailsByIdQueryHandlerTest()
        {
            _tenatApplicationDetailsRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
            _queryRestClientMock = new Mock<IQueryRestClient>();
            _restClientOptionsMock = new Mock<IOptions<RestClientOptions>>();

            _restClientOptionsMock.Setup(x => x.Value).Returns(Mock.Of<RestClientOptions>(x => x.ThirdPartyIntegration == "https://test.test"));
            _queryRestClientMock.Setup(x => x.GetAsync<GetTenantVerifiedProfileDto>(It.IsAny<string>())).ReturnsAsync(new QueryResponse<GetTenantVerifiedProfileDto>(System.Net.HttpStatusCode.Conflict, "Invalid"));
            _loggerMock = new Mock<ILogger<GetTenantApplicationDetailsByIdQueryHandler>>();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantApplicationDetailsByIdQueryHandler(_tenatApplicationDetailsRepositoryMock.Object, _tenantVerificationProfileMock.Object, _loggerMock.Object, _queryRestClientMock.Object, _restClientOptionsMock.Object);
            var query = new QueryTenantApplicationDetailsById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantApplicationDetailsDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantApplicationDetailsByIdQueryHandler(_tenatApplicationDetailsRepositoryMock.Object, _tenantVerificationProfileMock.Object, _loggerMock.Object, _queryRestClientMock.Object,_restClientOptionsMock.Object);
            var query = new QueryTenantApplicationDetailsById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.CarsNumber.ShouldBe(0);
            result.PetsNumber.ShouldBe(0);
            result.Occupants.Adults.ShouldBe(1);
            result.Occupants.Children.ShouldBe(0);
            result.EstimatedMove.ShouldBeNull();
        }
    }
}

using System;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ResponseMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;
namespace Test.UserService.Queries
{
	public class GetTenantApplicationListPageByIdQueryHandlerTest
	{
        private readonly Mock<IRepository<TenantPropertyApplication, Guid>> _tenantPropertyApplicationRepository;
        private readonly Mock<IQueryRestClient> _queryRestClientMock;
        private readonly Mock<IOptions<RestClientOptions>> _restClientOptionsMock;
        private readonly Mock<IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto>> _getTenantProfileImageByIdHandler;
        private readonly Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>> _getTenantPersonalInfoByIdHandler;
        private readonly Mock<IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>> _getTenantApllicationDetailsByIdHandler;

        public GetTenantApplicationListPageByIdQueryHandlerTest()
		{
            _tenantPropertyApplicationRepository = MockTenantPropertyApplicationtRepository.GetTenantPropertyApplicationRepository();
            _queryRestClientMock = MockQueryRestClient_GetPropertyDetailstoApplyDto.GetMockAcceptedIQueryRestClient();
            _restClientOptionsMock = MockRestClientOptions.GetIOptionsMock();
            _getTenantProfileImageByIdHandler = MockTenantProfileImageQueryHandler.GetTenantProfileImageQueryHandler();
            _getTenantPersonalInfoByIdHandler = MockTenantBasicInformationQueryHandler2.GetTenantBasicInfoQueryHandler();
            _getTenantApllicationDetailsByIdHandler = MockTenantApllicationDetailsQueryHandler.GetTenantApllicationDetailsQueryHandler();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantApplicationListPageByIdQueryHandler(_tenantPropertyApplicationRepository.Object,
              _queryRestClientMock.Object,
             _restClientOptionsMock.Object,
             _getTenantProfileImageByIdHandler.Object,
             _getTenantPersonalInfoByIdHandler.Object,
            _getTenantApllicationDetailsByIdHandler.Object);

            var query = new QueryTenantApplicationListPageById(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),1,5); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantApplicationListPageDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantApplicationListPageByIdQueryHandler(_tenantPropertyApplicationRepository.Object,
              _queryRestClientMock.Object,
             _restClientOptionsMock.Object,
             _getTenantProfileImageByIdHandler.Object,
             _getTenantPersonalInfoByIdHandler.Object,
            _getTenantApllicationDetailsByIdHandler.Object);

            var query = new QueryTenantApplicationListPageById(Guid.NewGuid(),1,5);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ApplicationListPage.Count.ShouldBeLessThanOrEqualTo(0);
            result.ShouldBeOfType<GetTenantApplicationListPageDto>();

        }
    }
}


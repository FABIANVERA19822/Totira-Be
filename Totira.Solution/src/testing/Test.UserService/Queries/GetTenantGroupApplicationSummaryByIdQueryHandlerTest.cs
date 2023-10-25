using System;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantGroupApplicationSummaryByIdQueryHandlerTest
	{
        private readonly Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>> GetTenantBasicInformationQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>>> GetTenantProfileProgressQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto>> GetTenantProfileImageQueryHandler;
        private readonly Mock<IRepository<TenantApplicationRequest, Guid>> GetTenantApplicationRequestRepository;

        public GetTenantGroupApplicationSummaryByIdQueryHandlerTest()
		{
            GetTenantBasicInformationQueryHandler = MockTenantBasicInformationQueryHandler2.GetTenantBasicInfoQueryHandler();
            GetTenantProfileProgressQueryHandler = MockTenantProfileProgressQueryHandler.GetTenantProfileProgressQueryHandler();
            GetTenantProfileImageQueryHandler = MockTenantProfileImageQueryHandler.GetTenantProfileImageQueryHandler();
            GetTenantApplicationRequestRepository = MockTenantApplicationRequestRepository.GetTenantApplicationRequestRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantGroupApplicationSummaryByIdQueryHandler(GetTenantProfileImageQueryHandler.Object,
              GetTenantProfileProgressQueryHandler.Object,
             GetTenantBasicInformationQueryHandler.Object,
             GetTenantApplicationRequestRepository.Object); 

            var query = new QueryTenantGroupApplicationSummaryById(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantGroupApplicationSummaryDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantGroupApplicationSummaryByIdQueryHandler(GetTenantProfileImageQueryHandler.Object,
                         GetTenantProfileProgressQueryHandler.Object,
                        GetTenantBasicInformationQueryHandler.Object,
                        GetTenantApplicationRequestRepository.Object);

            var query = new QueryTenantGroupApplicationSummaryById(Guid.NewGuid()); 

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.CoApplicants.ShouldBeNull();
            result.Guarantor.ShouldBeNull();
            result.MainApplicant.ShouldBeNull();


        }

    }
}


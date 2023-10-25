using System;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;

namespace Test.UserService.Queries
{
    public class GetTenantProfileSummaryByIdQueryHandlerTest
    {

        private readonly Mock<IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>> GetTenantApplicationDetailsQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>> GetTenantBasicInformationQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto>> GetTenantContactInformationQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto>> GetTenantEmployeeIncomesQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto>> GetTenantRentalHistoriesQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto>> GetTenantFeedbackViaLandlordQueryHandler;
        private readonly Mock<IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto>> GetTenantAcquaintanceReferralQueryHandler;
        private readonly Mock<IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto>> GetTenantAcquaintanceReferralFormInfoQueryHandler;

        public GetTenantProfileSummaryByIdQueryHandlerTest()
        {
            GetTenantApplicationDetailsQueryHandler = MockTenantApplicationDetailsQueryHandler.GetTenantApplicationDetailsQueryHandler();
            GetTenantBasicInformationQueryHandler = MockTenantBasicInformationQueryHandler.GetTenantBasicInformationQueryHandler();
            GetTenantContactInformationQueryHandler = MockTenantContactInformationQueryHandler.GetTenantContactInformationQueryHandler();
            GetTenantEmployeeIncomesQueryHandler = MockTenantEmployeeIncomesQueryHandler.GetTenantEmployeeIncomesQueryHandler();
            GetTenantRentalHistoriesQueryHandler = MockTenantRentalHistoriesQueryHandler.GetTenantRentalHistoriesQueryHandler();
            GetTenantFeedbackViaLandlordQueryHandler = MockTenantFeedbackViaLandlordQueryHandler.GetTenantFeedbackViaLandlordQueryHandler();
            GetTenantAcquaintanceReferralQueryHandler = MockTenantAcquaintanceReferralQueryHandler.GetTenantAcquaintanceReferralQueryHandler();
            GetTenantAcquaintanceReferralFormInfoQueryHandler = MockTenantAcquaintanceReferralFormInfoQueryHandler.GetTenantAcquaintanceReferralFormInfoQueryHandler();
        }



        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantProfileSummaryByIdQueryHandler(GetTenantApplicationDetailsQueryHandler.Object,
                GetTenantBasicInformationQueryHandler.Object,
                GetTenantContactInformationQueryHandler.Object,
                GetTenantEmployeeIncomesQueryHandler.Object,
               GetTenantRentalHistoriesQueryHandler.Object,
              GetTenantFeedbackViaLandlordQueryHandler.Object,
              GetTenantAcquaintanceReferralQueryHandler.Object,
              GetTenantAcquaintanceReferralFormInfoQueryHandler.Object);
            var query = new QueryTenantProfileSummaryById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantProfileSummaryDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantProfileSummaryByIdQueryHandler(GetTenantApplicationDetailsQueryHandler.Object,
                GetTenantBasicInformationQueryHandler.Object,
                GetTenantContactInformationQueryHandler.Object,
                GetTenantEmployeeIncomesQueryHandler.Object,
               GetTenantRentalHistoriesQueryHandler.Object,
              GetTenantFeedbackViaLandlordQueryHandler.Object,
              GetTenantAcquaintanceReferralQueryHandler.Object,
              GetTenantAcquaintanceReferralFormInfoQueryHandler.Object);
            var query = new QueryTenantProfileSummaryById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.RentalHistories.ShouldBeNull();
            result.RentalHistoriesfeedback.Count.ShouldBe(0);
            result.OtherReferrals.Count.ShouldBe(0);
            result.BasicInformation.ShouldBeNull();
            result.ApplicationDetails.ShouldBeNull();
            result.ContactInformation.ShouldBeNull();
            result.EmployeeIncomes.ShouldBeNull();

        }
    }
}


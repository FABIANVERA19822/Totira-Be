using System;
using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.DTO;
using System.Diagnostics.Metrics;
using Totira.Bussiness.UserService.Handlers.Queries;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantApplicationDetailsQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>> GetTenantApplicationDetailsQueryHandler()
        {
            #region MockingData
            var tenantApplicationDetails = new List<GetTenantApplicationDetailsDto>
            {
                new GetTenantApplicationDetailsDto
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")

                },


            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantApplicationDetailsById>()))
                    .ReturnsAsync((QueryTenantApplicationDetailsById id) => tenantApplicationDetails.Where(x => x.Id == id.Id).SingleOrDefault());

            #endregion

            return mockRepo;
        }

    }

    public static class MockTenantBasicInformationQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>> GetTenantBasicInformationQueryHandler()
        {
            #region MockingData
            var tenantBasicInformation = new List<GetTenantBasicInformationDto>
            {
                new GetTenantBasicInformationDto(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")),


            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantBasicInformationById>()))
                    .ReturnsAsync((QueryTenantBasicInformationById id) => tenantBasicInformation.Where(x => x.Id == id.TenantId).SingleOrDefault());

            #endregion

            return mockRepo;
        }

    }

    public static class MockTenantContactInformationQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto>> GetTenantContactInformationQueryHandler()
        {
            #region MockingData
            var tenantContactInformation = new List<GetTenantContactInformationDto>
            {
               new GetTenantContactInformationDto
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")

                },


            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantContactInformationByTenantId>()))
                    .ReturnsAsync((QueryTenantContactInformationByTenantId id) => tenantContactInformation.Where(x => x.Id == id.TenantId).SingleOrDefault());

            #endregion

            return mockRepo;
        }
    }

    public static class MockTenantEmployeeIncomesQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto>> GetTenantEmployeeIncomesQueryHandler()
        {
            #region MockingData
            var tenantEmployeeIncomes = new List<GetTenantEmployeeIncomesDto>
            {
               new GetTenantEmployeeIncomesDto(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")),






            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantEmployeeIncomesById>()))
                    .ReturnsAsync((QueryTenantEmployeeIncomesById id) => tenantEmployeeIncomes.Where(x => x.TenantId == id.TenantId).SingleOrDefault());

            #endregion

            return mockRepo;
        }
    }

    public static class MockTenantRentalHistoriesQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto>> GetTenantRentalHistoriesQueryHandler()
        {
            #region MockingData
            var tenantRentalHistories = new List<GetTenantRentalHistoriesDto>
            {
                new GetTenantRentalHistoriesDto
                {
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    RentalHistories = new List<TenantRentalHistoryDto>
                    {
                        new TenantRentalHistoryDto(new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),new Totira.Bussiness.UserService.DTO.CustomDate(5,2000),true,
                       new Totira.Bussiness.UserService.DTO.CustomDate(5,2000),"country","state","city","address","unit","zipcode","status",
                       new Totira.Bussiness.UserService.DTO.LandlordContactInformation("relationship","firstname","lastname",new Totira.Bussiness.UserService.DTO.RentalHistoriesPhoneNumber("123","02"),"email"))
                    }

                },






            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantRentalHistoriesById>()))
                    .ReturnsAsync((QueryTenantRentalHistoriesById id) => tenantRentalHistories.Where(x => x.TenantId == id.Id).SingleOrDefault());

            #endregion

            return mockRepo;
        }
    }

    public static class MockTenantFeedbackViaLandlordQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto>> GetTenantFeedbackViaLandlordQueryHandler()
        {
            #region MockingData
            var tenantFeedbackViaLandlord = new List<GetTenantFeedbackViaLandlordDto>
            {
                new GetTenantFeedbackViaLandlordDto
                {
                   StarScore = 5,

                },






            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantFeedbackViaLandlordById>()))
                    .ReturnsAsync((QueryTenantFeedbackViaLandlordById id) => tenantFeedbackViaLandlord.FirstOrDefault());

            #endregion

            return mockRepo;
        }
    }

    public static class MockTenantAcquaintanceReferralQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto>> GetTenantAcquaintanceReferralQueryHandler()
        {
            #region MockingData
            var tenantAcquaintanceReferral = new List<GetTenantAquaintanceReferralDto>
            {
                new GetTenantAquaintanceReferralDto
                {
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    AquaintanceReferrals = new List<AquaintanceReferralDto>
                    {
                        new AquaintanceReferralDto(new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),"fullname",
                        "email","relationship","phone","status"),
                    }






                },






            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantAcquaintanceReferralById>()))
                .ReturnsAsync((QueryTenantAcquaintanceReferralById id) => tenantAcquaintanceReferral.Where(x => x.TenantId == id.Id).SingleOrDefault());

            #endregion

            return mockRepo;
        }
    }

    public static class MockTenantAcquaintanceReferralFormInfoQueryHandler
    {
        public static Mock<IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto>> GetTenantAcquaintanceReferralFormInfoQueryHandler()
        {
            #region MockingData
            var tenantAcquaintanceReferralFormInfo = new List<GetAcquaintanceReferralFormInfoDto>
            {
                new GetAcquaintanceReferralFormInfoDto
                {
                   ReferralId = new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                   StarScore = 5,

                },






            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryAcquaintanceReferralFormInfoByReferralId>()))
                    .ReturnsAsync((QueryAcquaintanceReferralFormInfoByReferralId id) => tenantAcquaintanceReferralFormInfo.FirstOrDefault());

            #endregion

            return mockRepo;
        }
    }

}
using System;
using Moq;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.DTO;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantBasicInformationQueryHandler2
    {
        public static Mock<IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>> GetTenantBasicInfoQueryHandler()
        {
            #region MockingData
            var tenantBasicInformation = new List<GetTenantBasicInformationDto>
            {
                new GetTenantBasicInformationDto(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),


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

    public static class MockTenantProfileProgressQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>>> GetTenantProfileProgressQueryHandler()
        {
            #region MockingData
            var tenantProfileProgress = new List<Dictionary<string, int>>
            {
                new Dictionary<string, int>(),


            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantProfileProgressByTenantId>()))
                    .ReturnsAsync((QueryTenantProfileProgressByTenantId id) => tenantProfileProgress.FirstOrDefault());

            #endregion

            return mockRepo;
        }

    }

    public static class MockTenantProfileImageQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto>> GetTenantProfileImageQueryHandler()
        {
            #region MockingData
            var tenantProfileImage = new List<GetTenantProfileImageDto>
            {
                new GetTenantProfileImageDto(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), new GetTenantProfileImageDto.ProfileImageFile(string.Empty,string.Empty,string.Empty)),


            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.HandleAsync(It.IsAny<QueryTenantProfileImageById>()))
                    .ReturnsAsync((QueryTenantProfileImageById id) => tenantProfileImage.Where(x => x.Id == id.Id).SingleOrDefault());

            #endregion

            return mockRepo;
        }

    }

    public static class MockTenantApplicationRequestRepository
    {

        public static Mock<IRepository<TenantApplicationRequest, Guid>> GetTenantApplicationRequestRepository()
        {
            #region MockingData
            var tenantTenantApplicationRequests = new List<TenantApplicationRequest>
            {
                new TenantApplicationRequest
                {
                    TenantId= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),

                    CreatedBy = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantApplicationRequest, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(x => true))
                .ReturnsAsync(tenantTenantApplicationRequests);


            #endregion
            return mockRepo;
        }
    }
}


using System;
using Moq;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
	public static class MockTenantApllicationDetailsQueryHandler
    {
        public static Mock<IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>> GetTenantApllicationDetailsQueryHandler()
        {
            #region MockingData
            var tenantApplicationDetails = new List<GetTenantApplicationDetailsDto>
            {
                new GetTenantApplicationDetailsDto()
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
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
    public static class MockTenantPropertyApplicationtRepository
    {

        public static Mock<IRepository<TenantPropertyApplication, Guid>> GetTenantPropertyApplicationRepository()
        {
            #region MockingData
            var tenantPropertyApplications = new List<TenantPropertyApplication>
            {
                new TenantPropertyApplication
                {
                    PropertyId = "C5799045",
                    ApplicantId= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),

                    MainTenantId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    CreatedOn = DateTime.Now,
                    CoApplicantsIds = new List<Guid>()

        }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantPropertyApplication, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(x => true))
                .ReturnsAsync(tenantPropertyApplications);


            #endregion
            return mockRepo;
        }
    }
}



using Moq;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantApplicationDetailsRepository
    {
        public static Mock<IRepository<TenantApplicationDetails, Guid>> GetTenantApplicationDetailsRepository()
        {
            #region MockingData
            var tenantApplicationDetails = new List<TenantApplicationDetails>
            {
                new TenantApplicationDetails
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    EstimatedMove = new ApplicationDetailEstimatedMove(4,2023),
                    EstimatedRent = "0",
                    Occupants = new ApplicationDetailOccupants(2,1),
                    Smoker = false,
                    Pets = new List<ApplicationDetailPet>(),
                    Cars = new List<ApplicationDetailCar>(),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null
                },
                new TenantApplicationDetails
                {
                    Id =  new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                    TenantId =  new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                    EstimatedMove = new ApplicationDetailEstimatedMove(4,2023),
                    EstimatedRent = "0",
                    Occupants = new ApplicationDetailOccupants(2,1),
                    Smoker = false,
                    Pets = new List<ApplicationDetailPet>(),
                    Cars = new List<ApplicationDetailCar>(),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantApplicationDetails, Guid>>();
            #endregion

            #region SetupAllMethods


            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantApplicationDetails, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantApplicationDetails, bool>> expression) => tenantApplicationDetails.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tenantApplicationDetails.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantApplicationDetails>()))
                    .Callback<TenantApplicationDetails>(sa => tenantApplicationDetails.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantApplicationDetails>()))
                    .Callback<TenantApplicationDetails>( entity =>
                    {
                        var applicationToUpdate = tenantApplicationDetails.Where(x => x.Id == entity.Id).SingleOrDefault();
                        applicationToUpdate = entity;
                    });

            #endregion

            return mockRepo;
        }

    }
}

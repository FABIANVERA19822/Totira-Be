using Moq;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantEmploymentReferenceRepository
    {
        public static Mock<IRepository<TenantEmploymentReference, Guid>> GetTenantTenantEmploymentReferenceRepository()
        {
            #region MockingData
            var tenantEmploymentReferences = new List<TenantEmploymentReference>()
            {
                new TenantEmploymentReference
                {
                    Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    JobTitle = "TestJobTitle",
                    Relationship = "TestRelationship",
                    Email = "TestEmail@test.test",
                    PhoneNumber = new EmploymentReferencePhoneNumber("TestNumber","TestCountryCode"),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };

            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantEmploymentReference, Guid>>();
            #endregion

            #region SetupAllMethods
            mockRepo.Setup(r => r.Get(x => true))
                .ReturnsAsync(tenantEmploymentReferences);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => tenantEmploymentReferences.Where(x => x.Id == id).Single());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantEmploymentReference>()))
               .Callback<TenantEmploymentReference>(sa => tenantEmploymentReferences.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantEmploymentReference>()))
                    .Verifiable();
            #endregion
            return mockRepo;
        }

    }
}


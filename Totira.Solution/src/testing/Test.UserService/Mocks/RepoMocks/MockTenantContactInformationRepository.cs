using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantContactInformationRepository
    {

        public static Mock<IRepository<TenantContactInformation, Guid>> GetTenantContactInformationRepository()
        {
            #region MockingData
            var tenantContactInformations = new List<TenantContactInformation>
            {
                new TenantContactInformation
                {
                    TenantId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    Country = "CountyrTest",
                    Province = "ProvinceTest",
                    City = "CityTest",
                    Email = "testymactest@test.test",
                    ZipCode = "7414",
                    StreetAddress = "Fake Street 123",
                    HousingStatus = "Renting",
                    PhoneNumber = new ContactInformationPhoneNumber("Test","UpdateTest" ),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantContactInformation, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantContactInformation, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantContactInformation, bool>> expression) => tenantContactInformations.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Guid id) => tenantContactInformations.Where(x => x.Id == id).Single());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantContactInformation>()))
                    .Callback<TenantContactInformation>(sa => tenantContactInformations.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantContactInformation>()))
                    .Verifiable();

            #endregion
            return mockRepo;
        }
    }
}

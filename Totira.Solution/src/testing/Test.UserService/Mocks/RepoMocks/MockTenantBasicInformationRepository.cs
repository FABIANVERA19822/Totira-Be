using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantBasicInformationRepository
    {
        public static Mock<IRepository<TenantBasicInformation, Guid>> GetTenantBasicInformationRepository()
        {
            #region MockingData
            var tenantBasicInformations = new List<TenantBasicInformation>
            {
                new TenantBasicInformation
                {
                    Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    FirstName = "Testy",
                    LastName = "MacTest",
                    TenantEmail = "testymactest@test.test",
                    Birthday = new Totira.Bussiness.UserService.Domain.Common.Birthday(1993,27,03) ,
                    AboutMe = "i'm me",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantBasicInformation, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantBasicInformation, bool>>>()))
                    .ReturnsAsync((Expression<Func<TenantBasicInformation, bool>> expression) => tenantBasicInformations.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tenantBasicInformations.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantBasicInformation>()))
                    .Callback<TenantBasicInformation>(sa => tenantBasicInformations.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantBasicInformation>()))
                    .Verifiable();

            #endregion
            return mockRepo;
        }

    }
}

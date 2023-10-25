using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantPersonalInformationRepository
    {
        public static Mock<IRepository<TenantBasicInformation, Guid>> GetTenantPersonalInformationRepository()
        {
            #region MockingData
            var tenantPersonalInformations = new List<TenantBasicInformation>
            {
                new TenantBasicInformation
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    FirstName = "Testy",
                    LastName = "MacTest",
                    Birthday = new Birthday(1993,27,03),
                    SocialInsuranceNumber= "123456789",
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
            mockRepo.Setup(r => r.Get(x => true))
           .ReturnsAsync(tenantPersonalInformations);

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantBasicInformation, bool>>>()))
                   .ReturnsAsync((Expression<Func<TenantBasicInformation, bool>> expression) =>
                   tenantPersonalInformations.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.Get(x => true))
                    .ReturnsAsync(tenantPersonalInformations);
            
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tenantPersonalInformations.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantBasicInformation>()))
                    .Callback<TenantBasicInformation>(sa => tenantPersonalInformations.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantBasicInformation>()))
                    .Verifiable();
            #endregion

            return mockRepo;
        }

    }
}
 
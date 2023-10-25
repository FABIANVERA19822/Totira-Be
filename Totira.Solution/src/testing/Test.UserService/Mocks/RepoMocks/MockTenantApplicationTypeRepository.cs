

using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Handlers.Queries;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantApplicationTypeRepository
    {
        public static Mock<IRepository<TenantApplicationType, Guid>> GetTenantApplicationTypeRepository()
        {
            #region MockingData
            var tenantApplicationType = new List<TenantApplicationType>()
            {
                new TenantApplicationType()
                {   
                    Id = new Guid("E471131F-60C0-46F6-A980-11A37BE97473"),
                    TenantId =  new Guid("E471131F-60C0-46F6-A980-11A37BE97473"),
                    ApplicationType="Single"
                }
           };
             
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantApplicationType, Guid>>();
            #endregion

            #region SetupAllMethods
            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantApplicationType, bool>>>()))
            .ReturnsAsync((Expression<Func<TenantApplicationType, bool>> expression) => tenantApplicationType.Where(expression.Compile()).ToList());


            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((Guid id) => tenantApplicationType.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantApplicationType>()))
                    .Callback<TenantApplicationType>(sa => tenantApplicationType.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantApplicationType>()))
                    .Verifiable();

            #endregion

            return mockRepo;
        }
    }
}

using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks;

public static class MockGroupApplicationRepository
{
    public static Mock<IRepository<TenantGroupApplicationProfile, Guid>> GetGroupApplicationRepository()
    {
        #region MockingData
        var tenantGroupApplicationProfile = new List<TenantGroupApplicationProfile>
        {
            new TenantGroupApplicationProfile()
            {
               Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
               TenantId=new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"),
               FirstName="Azza",
               Email="Azza@totira.com",
               InvinteeType=1,
               Status=1,
               IsVerifiedEmailConfirmation=false,
               CreatedOn=DateTime.Now,
            },

             new TenantGroupApplicationProfile
             {
               Id= new Guid("CF0A8C1C-FFFF-41A1-A12C-53D9BE513A1C"),
               TenantId=new Guid("FFFFFFFF-F2D1-41A1-A12C-53D9BE513A1C"),
               FirstName="Mona",
               Email="Mona12@Gmail.com",
            } };
        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRepository<TenantGroupApplicationProfile, Guid>>();
        #endregion

        #region SetupAllMethods
        mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantGroupApplicationProfile, bool>>>()))
                .ReturnsAsync((Expression<Func<TenantGroupApplicationProfile, bool>> expression) =>
                               tenantGroupApplicationProfile.Where(expression.Compile()).ToList());
        return mockRepo;
        #endregion
    }
}




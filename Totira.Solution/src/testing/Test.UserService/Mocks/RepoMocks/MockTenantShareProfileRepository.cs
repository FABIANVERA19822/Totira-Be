using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantShareProfileRepository
    {
        public static Mock<IRepository<TenantShareProfile, Guid>> GetTenantShareProfileRepository()
        {
            #region MockingData
            var tenantShareProfile = new List<TenantShareProfile>
            {
                new TenantShareProfile
                {
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    EncryptedAccessCode="rf-sv4SIwaBQaLSm80kdCw",
                    Email="test12@test.com",
                    CreatedOn=DateTime.Now,
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"),
                    Message=null,
                    CreatedBy=new Guid("{48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC}"),
                    IsAccaptTermsAndConditions=false

                },
                new TenantShareProfile
                {
                    Id =new Guid("{48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC}"),
                    CreatedBy = new Guid("{48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC}"),
                    TenantId =new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1F"),
                    EncryptedAccessCode="rf-sv4SIwaBQaLSm80kdCw",
                    Email="test123@test.com",
                    Message="Message",
                    CreatedOn=DateTime.Now,
                    IsAccaptTermsAndConditions=false
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantShareProfile, Guid>>();
            #endregion



            #region SetupAllMethods
            mockRepo.Setup(r => r.Get(x => true))
           .ReturnsAsync(tenantShareProfile);

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantShareProfile, bool>>>()))
                   .ReturnsAsync((Expression<Func<TenantShareProfile, bool>> expression) =>
                   tenantShareProfile.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.Get(x => true))
                    .ReturnsAsync(tenantShareProfile);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tenantShareProfile.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantShareProfile>()))
                    .Callback<TenantShareProfile>(sa => tenantShareProfile.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantShareProfile>()))
                    .Verifiable();
            #endregion


            return mockRepo;
        }
    }
}

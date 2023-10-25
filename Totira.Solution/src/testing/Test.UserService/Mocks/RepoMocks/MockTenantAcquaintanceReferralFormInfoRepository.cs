using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantAcquaintanceReferralFormInfoRepository
    {
        public static Mock<IRepository<TenantAcquaintanceReferralFormInfo, Guid>> GetTenantAcquaintanceReferralFormInfoRepository()
        {
            #region MockingData
            var tentantAcquaintanceReferralFormInfo = new List<TenantAcquaintanceReferralFormInfo>
            {
                new TenantAcquaintanceReferralFormInfo
                {   Id = new Guid("{75CC87A6-EF71-491C-BECE-CA3C5FE1DB94}"),
                    TenantId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    ReferralId =new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                    Score = 4,
                    Comment = "He is honest",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                },
                new TenantAcquaintanceReferralFormInfo
                {   Id= new Guid("{219BEE77-DD22-4116-B862-9A905C400FEB}"),
                    TenantId= new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                    ReferralId = new Guid("{219BEE77-DD22-4116-B862-9A905C400FEB}"),
                    Score = 1,
                    Comment = "He is Bad to deal with",
                    CreatedBy = new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region InstanciateTheMock
            var mockRepo = new Mock<IRepository<TenantAcquaintanceReferralFormInfo, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantAcquaintanceReferralFormInfo, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantAcquaintanceReferralFormInfo, bool>> expression) 
                        => tentantAcquaintanceReferralFormInfo.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tentantAcquaintanceReferralFormInfo.Where(x => x.ReferralId == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantAcquaintanceReferralFormInfo>()))
                    .Callback<TenantAcquaintanceReferralFormInfo>(sa => tentantAcquaintanceReferralFormInfo.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantAcquaintanceReferralFormInfo>()))
                    .Callback<TenantAcquaintanceReferralFormInfo>(sa => tentantAcquaintanceReferralFormInfo.Where(x => x.ReferralId == sa.ReferralId));

            #endregion

            return mockRepo;
        }

    }
}

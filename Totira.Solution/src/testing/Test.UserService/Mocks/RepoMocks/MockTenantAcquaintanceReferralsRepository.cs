using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantAcquaintanceReferralsRepository
    {
        public static Mock<IRepository<TenantAcquaintanceReferrals, Guid>> GetTenantAcquaintanceReferralsRepository()
        {
            #region MockingData
            var tenantAcquaintanceReferrals = new List<TenantAcquaintanceReferrals>
            {
                new TenantAcquaintanceReferrals
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                    Referrals = new List<TenantAcquaintanceReferral>
                    {
                        new TenantAcquaintanceReferral
                        {
                            Id = new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                            TenantId = new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94"),
                            FullName = "Testy MacTest",
                            Email = "testyMacTest@test.test",
                            CreatedBy = Guid.NewGuid(),
                            CreatedOn = DateTime.Now
                        },
                        new TenantAcquaintanceReferral
                        {
                            Id = new Guid("{E471131F-60C0-46F6-A980-11A37BE97473}"),
                            TenantId = new Guid("{E471131F-60C0-46F6-A980-11A37BE97473}"),
                            FullName = "Testy MacTest the Third",
                            Email = "testyMacTest3@test.test",
                            CreatedBy = Guid.NewGuid(),
                            CreatedOn = DateTime.Now
                        }
                    }
                },
                new TenantAcquaintanceReferrals
                {
                    Id =new Guid("{48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC}"),
                    CreatedBy = new Guid("{48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC}"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                    Referrals = new List<TenantAcquaintanceReferral>
                    {
                        new TenantAcquaintanceReferral
                        {   Id = new Guid("{219BEE77-DD22-4116-B862-9A905C400FEB}"),
                            TenantId = new Guid("{219BEE77-DD22-4116-B862-9A905C400FEB}"),
                            FullName = "Testy MacTest",
                            Email = "testyMacTest@test.test",
                            CreatedBy = Guid.NewGuid(),
                            CreatedOn = DateTime.Now
                        }
                    }
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantAcquaintanceReferrals, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantAcquaintanceReferrals, bool>>>()))
                   .ReturnsAsync((Expression<Func<TenantAcquaintanceReferrals, bool>> expression) =>
                   tenantAcquaintanceReferrals.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync((Guid id) => tenantAcquaintanceReferrals.Where(x => x.Id == id).SingleOrDefault());


            mockRepo.Setup(r => r.Add(It.IsAny<TenantAcquaintanceReferrals>()))
                    .Callback<TenantAcquaintanceReferrals>(sa => tenantAcquaintanceReferrals.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantAcquaintanceReferrals>()))
                    .Verifiable();
            #endregion


            return mockRepo;
        }
    }
}

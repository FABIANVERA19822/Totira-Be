using System;
using Moq;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;

namespace Test.UserService.Mocks.RepoMocks
{
	public class MockTenantContactLandlordRepository
	{
        public static Mock<IRepository<TenantContactLandlord, Guid>> GetTenantContactLandlordRepository()
        {
            #region MockingData
            var tenantContactLandlords = new List<TenantContactLandlord>
            {
                new TenantContactLandlord
                {
                    TenantId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantContactLandlord, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantContactLandlord, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantContactLandlord, bool>> expression) => tenantContactLandlords.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Guid id) => tenantContactLandlords.Where(x => x.Id == id).Single());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantContactLandlord>()))
                    .Callback<TenantContactLandlord>(sa => tenantContactLandlords.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantContactLandlord>()))
                    .Verifiable();

            #endregion
            return mockRepo;
        }
    }
}


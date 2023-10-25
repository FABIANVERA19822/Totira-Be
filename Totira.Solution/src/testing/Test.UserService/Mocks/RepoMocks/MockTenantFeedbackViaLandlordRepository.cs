using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantFeedbackViaLandlordRepository
    {
        public static Mock<IRepository<TenantFeedbackViaLandlord, Guid>> GetTenantFeedbackViaLandlordRepository()
        {
            #region MockingData
            var tentantFeedbackViaLandlord = new List<TenantFeedbackViaLandlord>
            {
                new TenantFeedbackViaLandlord
                {
                    Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    LandlordId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"),
                    Score = 4,
                    Comment = "He is honest",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                },
                new TenantFeedbackViaLandlord
                {
                    Id= new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                    TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    LandlordId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"),
                    Score = 1,
                    Comment = "He is Bad to deal with",
                    CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1A"),
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                }
            };
            #endregion

            #region InstanciateTheMock
            var mockRepo = new Mock<IRepository<TenantFeedbackViaLandlord, Guid>>();
            #endregion

            #region SetupAllMethods




            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantFeedbackViaLandlord, bool>>>()))
                    .ReturnsAsync((Expression<Func<TenantFeedbackViaLandlord, bool>> expression) =>
                       tentantFeedbackViaLandlord.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tentantFeedbackViaLandlord.Where(x => x.LandlordId == id).FirstOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantFeedbackViaLandlord>()))
                    .Callback<TenantFeedbackViaLandlord>(sa => tentantFeedbackViaLandlord.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantFeedbackViaLandlord>()))
                    .Callback<TenantFeedbackViaLandlord>(sa => tentantFeedbackViaLandlord.Where(x => x.LandlordId == sa.LandlordId));

            #endregion

            return mockRepo;
        }

    }
}

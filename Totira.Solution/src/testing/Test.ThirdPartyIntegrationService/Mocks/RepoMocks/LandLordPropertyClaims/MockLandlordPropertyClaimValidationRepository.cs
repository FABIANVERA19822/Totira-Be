using Moq;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using static Totira.Support.Persistance.IRepository;

namespace Test.ThirdPartyIntegrationService.Mocks.RepoMocks.LandLordPropertyClaims
{
    public static class MockLandlordPropertyClaimValidationRepository
    {
        public static Mock<IRepository<LandlordPropertyClaimValidation, string>> GetMockLandlordPropertyClaimValidationRepository()
        {
            #region MockingData

            var landlordPropertyClaimValidation = new List<LandlordPropertyClaimValidation>
            {
                new LandlordPropertyClaimValidation{
                Id = "1234567",
                Address = "123 Pepito Street",
                ClaimId = Guid.NewGuid(),
                Comments = new List<string> { },
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                Email = "xxx@email.com",
                LandlordId = Guid.NewGuid(),
                FinalDecision = string.Empty,
                Ml_num = string.Empty,
                Status = string.Empty,
                UpdatedBy = Guid.NewGuid(),
                UpdatedOn = DateTime.UtcNow,
                },
                new LandlordPropertyClaimValidation{
                Id = "12345678",
                Address = "123 Pepito Street",
                ClaimId = Guid.NewGuid(),
                Comments = new List<string> { },
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                Email = "xxx@email.com",
                LandlordId = Guid.NewGuid(),
                FinalDecision = string.Empty,
                Ml_num = string.Empty,
                Status = string.Empty,
                UpdatedBy = Guid.NewGuid(),
                UpdatedOn = DateTime.UtcNow,
                }
            };

            #endregion MockingData

            #region instanciateTheMock

            var mockRepo = new Mock<IRepository<LandlordPropertyClaimValidation, string>>();

            #endregion instanciateTheMock

            #region SetupAllMethods

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => landlordPropertyClaimValidation.Where(x => x.Id == id).Single());

            mockRepo.Setup(r => r.Add(It.IsAny<LandlordPropertyClaimValidation>()))
               .Callback<LandlordPropertyClaimValidation>(sa => landlordPropertyClaimValidation.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<LandlordPropertyClaimValidation>()))
                    .Verifiable();

            #endregion SetupAllMethods

            return mockRepo;
        }

        //LandlordPropertyClaimValidation
    }
}
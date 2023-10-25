using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public static class MockTenantRentalHistoriesRepository
    {
        public static Mock<IRepository<TenantRentalHistories, Guid>> GetTenantRentalHistoriesRepository()
        {
            #region MockingData
            var tentantRentalHistorial = new List<TenantRentalHistories>
            {
                new TenantRentalHistories
                {
                    Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                    RentalHistories = new List<TenantRentalHistory>
                    {
                        new TenantRentalHistory
                        {
                            Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            RentalStartDate = new CustomDate(03,2022),
                            RentalEndDate = new CustomDate(03,2023),
                            Country = "Argentina",
                            State = "Misiones",
                            City = "Posadas",
                            Address = "Calle Falsa 123",
                            Unit = "House",
                            ContactInformation = new LandlordContactInformation("test", "LandlorTest", "More Test",
                                                                                new RentalHistoriesPhoneNumber("phone Test","code Test"),
                                                                                "test@test.test"),
                            CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = null,
                            UpdatedOn = null
                        },
                        new TenantRentalHistory
                        {
                            Id= new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                            TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            RentalStartDate = new CustomDate(03,2022),
                            RentalEndDate = new CustomDate(03,2023),
                            Country = "Argentina",
                            State = "Misiones",
                            City = "Posadas",
                            Address = "Calle Falsa 123",
                            Unit = "House",
                            ContactInformation = new LandlordContactInformation("test", "LandlorTest", "More Test",
                                                                                new RentalHistoriesPhoneNumber("phone Test","code Test"),
                                                                                "test@test.test"),
                            CreatedBy = new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                            CreatedOn = DateTime.Now,
                            UpdatedBy = null,
                            UpdatedOn = null
                        }
                    }
                },
                new TenantRentalHistories { Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1F"), RentalHistories = new List<TenantRentalHistory>(){
                    new TenantRentalHistory()
                    {
                        Id= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"),
                            RentalStartDate = new CustomDate(03,2022),
                            RentalEndDate = new CustomDate(03,2023),
                            Country = "Argentina",
                            State = "Misiones",
                            City = "Posadas",
                            Address = "Calle Falsa 123",
                            Unit = "House",
                            ContactInformation = new LandlordContactInformation("test", "LandlorTest", "More Test",
                                                                                new RentalHistoriesPhoneNumber("phone Test","code Test"),
                                                                                "test@test.test"),

                    }
                } }
            };
            #endregion

            #region InstanciateTheMock
            var mockRepo = new Mock<IRepository<TenantRentalHistories, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(x => true))
                    .ReturnsAsync(tentantRentalHistorial);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => tentantRentalHistorial.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantRentalHistories>()))
                    .Callback<TenantRentalHistories>(sa => tentantRentalHistorial.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantRentalHistories>()))
                    .Callback<TenantRentalHistories>(sa => tentantRentalHistorial.Where(x => x.Id == sa.Id));

            var history = new TenantRentalHistory
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                RentalStartDate = new CustomDate(03, 2022),
                RentalEndDate = new CustomDate(03, 2023),
                Country = "Argentina",
                State = "Misiones",
                City = "Posadas",
                Address = "Calle Falsa 123",
                Unit = "House",
                ContactInformation = new LandlordContactInformation("test", "LandlorTest", "More Test",
                                                                                new RentalHistoriesPhoneNumber("phone Test", "code Test"),
                                                                                "test@test.test"),
                CreatedBy = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = null
            };


            List<TenantRentalHistories> baseMock = new List<TenantRentalHistories>();

            TenantRentalHistories objReturn = new TenantRentalHistories()
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                RentalHistories = new List<TenantRentalHistory>() { history },
            };

            baseMock.Add(objReturn);


            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantRentalHistories, bool>>>())).Returns(Task.FromResult(baseMock.AsEnumerable()));


            #endregion

            return mockRepo;
        }

    }
}

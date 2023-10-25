using Moq;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantEmployeeIncomesRepository
    {
        public static Mock<IRepository<TenantEmployeeIncomes, Guid>> GetTenantEmployeeIncomesRepository()
        {
            #region MockingData

            var mockedData = new List<TenantEmployeeIncomes>();

            var tenant1Incomes = TenantEmployeeIncomes.Create(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            tenant1Incomes.EmployeeIncomes!.AddRange(new List<TenantEmployeeIncome>
            {
                TenantEmployeeIncome.Create(
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            "Company Name",
                            "Position",
                            DateTime.Today.AddYears(-1),
                            true,
                            DateTime.Today,
                            2500,
                            EmploymentContactReference.Create("Reference1",
                                                              "Reference1",
                                                              "Reference1Position",
                                                              "Reference1Relationship",
                                                              "fakemail@mail.com",
                                                              EmploymentContactReferencePhoneNumber.Create("reference1Phone","ref1CountryCode")),
                            new List<Totira.Bussiness.UserService.Domain.Common.File>
                            {
                                Totira.Bussiness.UserService.Domain.Common.File.Create("fakefile","fakekey","",1)
                            }),
                TenantEmployeeIncome.Create(
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513555"),
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            "Company2 Name",
                            "Position2",
                            DateTime.Today.AddYears(-1),
                            false,
                            DateTime.Today,
                            2500,
                            EmploymentContactReference.Create("Reference1",
                                                              "Reference1",
                                                              "Reference1Position",
                                                              "Reference1Relationship",
                                                              "fakemail@mail.com",
                                                              EmploymentContactReferencePhoneNumber.Create("reference1Phone","ref1CountryCode")),
                            new List<Totira.Bussiness.UserService.Domain.Common.File>
                            {
                                Totira.Bussiness.UserService.Domain.Common.File.Create("fakefile","fakekey","",1)
                            })
            });
            var tenant2Incomes = TenantEmployeeIncomes.Create(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE522222"));
            tenant2Incomes.EmployeeIncomes!.AddRange(new List<TenantEmployeeIncome>
            {
                TenantEmployeeIncome.Create(
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                            new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE522222"),
                            "Company Name",
                            "Position",
                            DateTime.Today.AddYears(-1),
                            false,
                            DateTime.Today,
                            2500,
                            EmploymentContactReference.Create("Reference1",
                                                              "Reference1",
                                                              "Reference1Position",
                                                              "Reference1Relationship",
                                                              "fakemail@mail.com",
                                                              EmploymentContactReferencePhoneNumber.Create("reference1Phone","ref1CountryCode")),
                            new List<Totira.Bussiness.UserService.Domain.Common.File>
                            {
                                Totira.Bussiness.UserService.Domain.Common.File.Create("fakefile","fakekey","",1)
                            })
            });

            mockedData.Add(tenant1Incomes);
            mockedData.Add(tenant2Incomes);

            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantEmployeeIncomes, bool>>>()))
                        .ReturnsAsync((Expression<Func<TenantEmployeeIncomes, bool>> expression) => mockedData.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Guid id) => mockedData.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Add(It.IsAny<TenantEmployeeIncomes>()))
                    .Callback<TenantEmployeeIncomes>(sa => mockedData.Add(sa));

            mockRepo.Setup(r => r.Update(It.IsAny<TenantEmployeeIncomes>()))
                    .Verifiable();

            #endregion
            return mockRepo;
        }
    }
}

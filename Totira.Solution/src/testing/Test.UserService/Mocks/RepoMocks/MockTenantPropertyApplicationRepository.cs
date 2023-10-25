﻿
using Moq;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantPropertyApplicationRepository
    {
        public static List<TenantPropertyApplication> Values { get; } = new()
        {
            new TenantPropertyApplication {

                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                ApplicationRequestId =new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                ApplicantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                Status ="UnderRevision",
                PropertyId="C523423",
                CreatedOn = DateTime.Now
            },
            new TenantPropertyApplication
            {
                Id = Guid.Parse("1538ae98-bc44-4496-995f-d9528ad1fefb"),
                ApplicationRequestId = Guid.Parse("a196a475-0e02-4825-b384-4cce54a789b2"),
                ApplicantId = Guid.Parse("2c4091b8-c8dd-4763-bffb-49d92a6bfd1c"),
                PropertyId = "T1234567",
                Status = "UnderRevision",
                CreatedOn = DateTime.UtcNow
            }
        };

        public static Mock<IRepository<TenantPropertyApplication, Guid>> GetTenantPropertyApplicationRepository()
        {
            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantPropertyApplication, Guid>>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<TenantPropertyApplication, bool>>>()))
                      .ReturnsAsync((Expression<Func<TenantPropertyApplication, bool>> expression) => Values.Where(expression.Compile()).ToList());

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Guid id) => Values.Where(x => x.Id == id).SingleOrDefault());

            mockRepo.Setup(r => r.Update(It.IsAny<TenantPropertyApplication>()))
                    .Callback<TenantPropertyApplication>((item) =>
                    {
                        var found = Values.FirstOrDefault(e => e.Id == item.Id);
                        if (found is not null)
                        {
                            found.Status = item.Status;
                            found.UpdatedOn = item.UpdatedOn;
                        }
                    });

            mockRepo.Setup(r => r.Add(It.IsAny<TenantPropertyApplication>()))
                    .Callback<TenantPropertyApplication>(sa => Values.Add(sa));
            #endregion
            return mockRepo;
        }
    }
}
using Moq;
using Shouldly;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantEmploymentReferenceByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;

        public GetTenantEmploymentReferenceByIdQueryHandlerTest()
        {
            _tenantEmploymentReferenceRepositoryMock = new Mock<IRepository<TenantEmploymentReference, Guid>>();
        }
        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantEmploymentReferenceByIdQueryHandler(_tenantEmploymentReferenceRepositoryMock.Object);
            var query = new QueryTenantEmploymentReferenceById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantEmploymentReferenceDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantEmploymentReferenceByIdQueryHandler(_tenantEmploymentReferenceRepositoryMock.Object);
            var query = new QueryTenantEmploymentReferenceById(Guid.NewGuid()); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantEmploymentReferenceDto>();
            result.FirstName.ShouldBeEmpty();
            result.LastName.ShouldBeEmpty();
            result.JobTitle.ShouldBeEmpty();
            result.Relationship.ShouldBeEmpty();
            result.Email.ShouldBeEmpty();
            result.PhoneNumber.Number.ShouldBeEmpty();
            result.PhoneNumber.CountryCode.ShouldBeEmpty();

        }
    }
}


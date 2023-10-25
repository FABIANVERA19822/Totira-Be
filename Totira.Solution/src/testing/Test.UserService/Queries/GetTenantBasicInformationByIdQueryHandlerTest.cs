using Moq;
using Shouldly;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantBasicInformationByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenatPersonalInformationRepositoryMock;

        public GetTenantBasicInformationByIdQueryHandlerTest()
        {
            _tenatPersonalInformationRepositoryMock = new Mock<IRepository<TenantBasicInformation, Guid>>();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantBasicInformationByIdQueryHandler(_tenatPersonalInformationRepositoryMock.Object);
            var query = new QueryTenantBasicInformationById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantBasicInformationDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantBasicInformationByIdQueryHandler(_tenatPersonalInformationRepositoryMock.Object);
            var query = new QueryTenantBasicInformationById(Guid.NewGuid()); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantBasicInformationDto>();
            result.AboutMe.ShouldBeEmpty();
            result.FirstName.ShouldBeEmpty();
            result.LastName.ShouldBeEmpty();
            result.Birthday.Day.ShouldBe(0);
            result.Birthday.Month.ShouldBe(0);
            result.Birthday.Year.ShouldBe(0);
        }
    }
}

using Moq;
using Shouldly;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.PropertiesService.Queries;

public class GetPropertyMapQueryHandlerTest
{
    [Fact]
    public async Task HandleAsync_Should_ReturnOk()
    {
        //Arrange
        var repositoryMock = new Mock<IRepository<Property, string>>();
        var queryPropertyHandlerMock = new Mock<IQueryHandler<QueryProperty, GetPropertyDto>>();
        var query = new QueryPropertyMap(EnumPropertySortBy.Newest, 1, 10, new QueryFilter());

        repositoryMock
            .Setup(mock => mock.GetManyByIds(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(MockPropertyMapRepository.GetFiveOnlyWithLatitudeAndLongitude());

        queryPropertyHandlerMock
            .Setup(mock => mock.HandleAsync(It.IsAny<QueryProperty>()))
            .ReturnsAsync(MockGetPropertyDto.GetFiveAndSortByNewest());

        //Act
        var handler = new GetPropertyMapQueryHandler(repositoryMock.Object, queryPropertyHandlerMock.Object);
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldNotBeEmpty();
        result.Any(x => x.Info != null).ShouldBeTrue();
        result.Any(x => x.Info!.Photo != null).ShouldBeTrue();
    }
}

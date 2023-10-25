
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.DTO;
using Moq;
namespace Test.PropertiesService.Queries;

public class GetAllPropertiesQueryHandlerTest
{
    private readonly Mock<IRepository<Property, string>> _mockGetAllPropertiesRepository;

    public GetAllPropertiesQueryHandlerTest()
    {
        _mockGetAllPropertiesRepository = MockGetAllPropertiesRepository.GetAllProperties();
    }

    [Fact]
    public async Task HandleAsyncTest_OK()
    {
        //Arrange
        var handler = new QueryPropertyHandler(_mockGetAllPropertiesRepository.Object);
        QueryProperty query = new(EnumPropertySortBy.Newest, 1, 1,new QueryFilter());

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        Assert.NotEmpty(result.Properties);
        Assert.True(result.Properties.Count==1);
        Assert.IsType<GetPropertyDto>(result);

    }

    [Fact]
    public async Task HandleAsyncTest_NoData()
    {
        //Arrange
        var handler = new QueryPropertyHandler(_mockGetAllPropertiesRepository.Object);
           QueryProperty query = new(EnumPropertySortBy.Newest, 10, 1, new QueryFilter()); 

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        Assert.Empty(result.Properties);
        Assert.True(result.Properties.Count == 0);
        Assert.IsType<GetPropertyDto>(result);
    }
    [Fact]
    public async Task HandleAsyncTest_SortByNewest()
    {
        //Arrange
        var handler = new QueryPropertyHandler(_mockGetAllPropertiesRepository.Object);
        QueryProperty query = new(EnumPropertySortBy.Newest ,1,2, new QueryFilter());

        //Act
        var result = await handler.HandleAsync(query);

        //Assert

        Assert.NotEmpty(result.Properties);
        Assert.True(result?.Properties?.Count == 2);
        Assert.True(result?.Properties?.FirstOrDefault()?.Id == "1234ABCDE");
        Assert.IsType<GetPropertyDto>(result);
    }

    [Fact]
    public async Task HandleAsyncTest_SortByPriceLowToHigh()
    {
        //Arrange
        var handler = new QueryPropertyHandler(_mockGetAllPropertiesRepository.Object);
        QueryProperty query = new(EnumPropertySortBy.PriceLowToHigh, 1, 2, new QueryFilter());

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        Assert.NotEmpty(result.Properties);
        Assert.True(result.Properties.Count == 2);
        Assert.True(result?.Properties?.FirstOrDefault()?.Id == "12AB");
        Assert.IsType<GetPropertyDto>(result);
    }
    [Fact]
    public async Task HandleAsyncTest_SortByPriceHighToLow()
    {
        //Arrange
        var handler = new QueryPropertyHandler(_mockGetAllPropertiesRepository.Object);
        QueryProperty query = new(EnumPropertySortBy.PriceHighToLow, 1, 2, new QueryFilter());

        //Act
        var result = await handler.HandleAsync(query);

        //Assert

        Assert.NotEmpty(result.Properties);
        Assert.True(result.Properties.Count == 2);
        Assert.True(result?.Properties?.FirstOrDefault()?.Id == "1234ABCDE");
        Assert.IsType<GetPropertyDto>(result);
    }
}
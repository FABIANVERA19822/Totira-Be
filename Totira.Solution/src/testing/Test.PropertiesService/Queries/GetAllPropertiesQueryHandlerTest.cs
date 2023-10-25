﻿using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.DTO;
using Moq;
using Totira.Support.CommonLibrary.Interfaces;

namespace Test.PropertiesService.Queries;

public class GetAllPropertiesQueryHandlerTest : BaseQueryHandlerTest<QueryPropertyHandler>
{
    private readonly Mock<IRepository<Property, string>> _mockGetAllPropertiesRepository;
    private readonly Mock<IRepository<PropertyImages, string>> _mockGetPropertyImage;
    private readonly Mock<IS3Handler> _mockS3Handler;

    protected override QueryPropertyHandler QueryHandler => new QueryPropertyHandler(
        _mockGetAllPropertiesRepository.Object, _mockGetPropertyImage.Object, _mockS3Handler.Object);

    public GetAllPropertiesQueryHandlerTest()
    {
        _mockGetAllPropertiesRepository = MockGetAllPropertiesRepository.GetAllProperties();
        _mockGetPropertyImage = new Mock<IRepository<PropertyImages, string>>();
        _mockS3Handler = new Mock<IS3Handler>();
    }

    [Fact]
    public async Task HandleAsyncTest_OK()
    {
        //Arrange
        QueryProperty query = new(EnumPropertySortBy.Newest, 1, 1,new QueryFilter());

        //Act
        var result = await QueryHandler.HandleAsync(query);

        //Assert
        Assert.NotEmpty(result.Properties);
        Assert.True(result.Properties.Count==1);
        Assert.IsType<GetPropertyDto>(result);

    }

    [Fact]
    public async Task HandleAsyncTest_NoData()
    {
        //Arrange
        QueryProperty query = new(EnumPropertySortBy.Newest, 10, 1, new QueryFilter()); 

        //Act
        var result = await QueryHandler.HandleAsync(query);

        //Assert
        Assert.Empty(result.Properties);
        Assert.True(result.Properties.Count == 0);
        Assert.IsType<GetPropertyDto>(result);
    }
    [Fact]
    public async Task HandleAsyncTest_SortByNewest()
    {
        //Arrange
        QueryProperty query = new(EnumPropertySortBy.Newest ,1,2, new QueryFilter());

        //Act
        var result = await QueryHandler.HandleAsync(query);

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
        QueryProperty query = new(EnumPropertySortBy.PriceLowToHigh, 1, 2, new QueryFilter());

        //Act
        var result = await QueryHandler.HandleAsync(query);

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
        QueryProperty query = new(EnumPropertySortBy.PriceHighToLow, 1, 2, new QueryFilter());

        //Act
        var result = await QueryHandler.HandleAsync(query);

        //Assert

        Assert.NotEmpty(result.Properties);
        Assert.True(result.Properties.Count == 2);
        Assert.True(result?.Properties?.FirstOrDefault()?.Id == "1234ABCDE");
        Assert.IsType<GetPropertyDto>(result);
    }
}
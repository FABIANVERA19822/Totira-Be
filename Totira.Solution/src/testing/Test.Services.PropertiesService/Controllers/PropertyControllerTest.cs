using Moq;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Services.PropertiesService.Controllers;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Support.Application.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Bussiness.PropertiesService.DTO;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.Handlers.Commands;
using CrestApps.RetsSdk.Services;
using Totira.Bussiness.PropertiesService.Commands;
using Microsoft.AspNetCore.Mvc;
using Test.Services.PropertiesService.Mocks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Totira.Bussiness.PropertiesService.Handlers.Queries;

namespace Test.Services.PropertiesService.Controllers
{
    public class PropertyControllerTest
    {

        private readonly Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>> _getypropertydataByIdHandlerMock;
        private readonly Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>> _getLocationsBySearchKeword;
        private readonly Mock<IQueryHandler<QueryProperty, GetPropertyDto>> _getypropertydataHandler;
        private readonly Mock<IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>> _getpropertyDetailsdataHandler;
        private readonly Mock<IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>> _getpropertyCarrouselImagesHandler;
        private readonly Mock<ILogger<PropertyController>> _loggerMock;
        public PropertyControllerTest()
        {
            _getypropertydataByIdHandlerMock = new Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>>();
            _getypropertydataHandler = new Mock<IQueryHandler<QueryProperty, GetPropertyDto>>();
            _getLocationsBySearchKeword = new Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>>();
            _getpropertyDetailsdataHandler = new Mock<IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>>();
            _getpropertyCarrouselImagesHandler = new Mock<IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>>();
            _loggerMock = new Mock<ILogger<PropertyController>>();
        }

        [Fact]
        public async Task GetProperty_Ok()
        {
            //Arrange

            var query = "C523423";
            var querydata = new QueryPropertyByMlnum(query);
            var responseModel = StaticModels.GetPropertyData();

            var controller = MockPropertyData.GetPropertyControllerSut(
                out Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>> getypropertydataByIdHandlerMock,
                out Mock<IQueryHandler<QueryProperty, GetPropertyDto>> getypropertydataHandler,
                out Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>> getLocationsBySearchKeword,
                out Mock< IQueryHandler < QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>> getpropertyDetailsdataHandler,
                out Mock<IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>> getpropertyCarrouselImagesHandler,
                out Mock<IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>>> getPropertyMapHandler);

            getypropertydataByIdHandlerMock.Setup(ps => ps.HandleAsync(querydata)).Returns(Task.FromResult(responseModel));

            //Act
            var response = await controller.GetProperty(query);
            var result = response.Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(result);

        }

    }

}


using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Services.PropertiesService.Controllers;
using Microsoft.Extensions.Logging; 
namespace Test.Services.PropertiesService.Mocks
{
    public class MockPropertyData
    { 
        internal static PropertyController GetPropertyControllerSut(
              out Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>> getypropertydataByIdHandler,
              out Mock<IQueryHandler<QueryProperty, GetPropertyDto>> getypropertydataHandler,
              out Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>> getLocationsBySearchKeword,
              out Mock<IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>> getpropertyDetailsdataHandler,
              out Mock<IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>> getpropertycarrouselImagesHandler,
              out Mock<IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>>> getPropertyMapHandler
            )
        {
            getypropertydataByIdHandler = new Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>>();
            getypropertydataHandler = new Mock<IQueryHandler<QueryProperty, GetPropertyDto>>();
            getLocationsBySearchKeword = new Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>>();
            getpropertyDetailsdataHandler = new Mock<IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>>();
            getpropertycarrouselImagesHandler = new Mock<IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>>();
            getPropertyMapHandler = new Mock<IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>>>();
            var logger = new Mock<ILogger<PropertyController>>();

            return new PropertyController(
                logger: logger.Object,
                getypropertydataByIdHandler: getypropertydataByIdHandler.Object,
                getypropertydataHandler: getypropertydataHandler.Object,
                getpropertyDetailsdataHandler: getpropertyDetailsdataHandler.Object,
                getLocationsBySearchKeword: getLocationsBySearchKeword.Object,
                getpropertyCarrouselImagesHandler: getpropertycarrouselImagesHandler.Object,
                getPropertyMapHandler: getPropertyMapHandler.Object) ;
        }


    }
}

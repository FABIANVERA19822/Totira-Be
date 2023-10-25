
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Services.PropertiesService.Controllers;
using Microsoft.Extensions.Logging;
using CrestApps.RetsSdk.Models;
using CrestApps.RetsSdk.Services;

namespace Test.Services.PropertiesService.Mocks
{
    public class MockPropertyData
    {

        internal static PropertyController GetPropertyControllerSut(
              out Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>> getypropertydataByIdHandler
            , out Mock<IQueryHandler<QueryProperty, GetPropertyDto>> getypropertydataHandler,
              out Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>> getLocationsBySearchKeword



                                )
        {
            getypropertydataByIdHandler = new Mock<IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>>();
            getypropertydataHandler = new Mock<IQueryHandler<QueryProperty, GetPropertyDto>>();
            getLocationsBySearchKeword = new Mock<IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>>();
            var logger = new Mock<ILogger<PropertyController>>();

            return new PropertyController(logger.Object, getypropertydataByIdHandler.Object, 
                                          getypropertydataHandler.Object, getLocationsBySearchKeword.Object);
        }


    }
}


using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Bussiness.PropertiesService.DTO.GetPropertyDetailstoApplyDto;
using static Totira.Support.Persistance.IRepository;

namespace Test.PropertiesService.Queries
{
    public class GetPropertyDetailsToApplyQueryHandlerTest
    { 
        private readonly Mock<ILogger<GetPropertyDetailsToApplyQueryHandler>> _loggerMock;
        private readonly Mock<IRepository<Property, string>> _propertydataRepositoryMock;
        private readonly Mock<IRepository<PropertyImages, string>> _propertyImageRepositoryMock;
        private readonly Mock<IS3Handler> _s3HandlerMock;

        public GetPropertyDetailsToApplyQueryHandlerTest()
        {
            _loggerMock = new Mock<ILogger<GetPropertyDetailsToApplyQueryHandler>>();
            _propertydataRepositoryMock = MockPropertyRepository.GetPropertyRepository("ResidentialProperty");
            _propertyImageRepositoryMock = new Mock<IRepository<PropertyImages, string>>();
            _s3HandlerMock = new Mock<IS3Handler>();    

        }

        [Fact] 
        public async Task HandleAsyncTest_RequestOk_ReturnsOK()
        {
            //Arrange
            var handler = new GetPropertyDetailsToApplyQueryHandler(_loggerMock.Object, _propertydataRepositoryMock.Object, _propertyImageRepositoryMock.Object, _s3HandlerMock.Object);
            var query = new QueryPropertyDetailsToApply("C523423");

            //Act
              var result = await handler.HandleAsync(query);

            //Assert
            
             Assert.NotNull(result);
             Assert.Equal("C523423", result.Id);
             Assert.IsType<GetPropertyDetailstoApplyDto>(result);
             Assert.IsType<PropertyMainImage>(result.PropertyImageFile);

        }

        [InlineData("C12345")]
        [InlineData("")]
        [Theory]
        public async Task HandleAsyncTest_MissingId_ReturnsOK(string data)
        {
            //Arrange  
            var handler = new GetPropertyDetailsToApplyQueryHandler(_loggerMock.Object, _propertydataRepositoryMock.Object, _propertyImageRepositoryMock.Object, _s3HandlerMock.Object);
            var query = new QueryPropertyDetailsToApply(data);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result.Id);
            Assert.IsType<GetPropertyDetailstoApplyDto>(result);
            // Assert.IsType<PropertyMainImage>(result.PropertyImageFile);
            Assert.Null(result.PropertyImageFile);

        }
    }
}

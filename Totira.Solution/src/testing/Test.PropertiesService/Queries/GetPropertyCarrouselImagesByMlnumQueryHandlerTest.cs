using Microsoft.Extensions.Logging;
using Moq;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.CommonLibrary.Interfaces; 
using static Totira.Support.Persistance.IRepository;

namespace Test.PropertiesService.Queries
{
    public class GetPropertyCarrouselImagesByMlnumQueryHandlerTest
    {
        private readonly Mock<ILogger<GetPropertyCarrouselImagesByMlnumQueryHandler>> _loggerMock; 
        private readonly Mock<IRepository<PropertyImages, string>> _propertyImageRepositoryMock;
        private readonly Mock<IS3Handler> _s3HandlerMock;
        public GetPropertyCarrouselImagesByMlnumQueryHandlerTest()
        {
            _loggerMock = new Mock<ILogger<GetPropertyCarrouselImagesByMlnumQueryHandler>>();
            _propertyImageRepositoryMock = MockGetPropertyCarrouselImagesRepository.GetPropertyCarrouselImages_WhenPropertyExistsRepository();
            _s3HandlerMock = new Mock<IS3Handler>();
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOK()
        {
            //Arrange
            var handler = new GetPropertyCarrouselImagesByMlnumQueryHandler( _propertyImageRepositoryMock.Object, _loggerMock.Object, _s3HandlerMock.Object);
            var query = new QueryPropertyCarrouselImagesByMl_num("C523423");

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetPropertyCarrouselImagesDto>(result);
            Assert.IsType<List<Totira.Bussiness.PropertiesService.DTO.PropertyImage>>(result.CarrouselImages);
            result.CarrouselImages.Count().Equals(1);
        }

        [InlineData("C12345")]
        [InlineData("")]
        [Theory]
        public async Task HandleAsyncTest_MissingId_ReturnsOK(string data)
        {
            //Arrange
            var handler = new GetPropertyCarrouselImagesByMlnumQueryHandler(_propertyImageRepositoryMock.Object, _loggerMock.Object, _s3HandlerMock.Object);
            var query = new QueryPropertyCarrouselImagesByMl_num(data);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetPropertyCarrouselImagesDto>(result);
            Assert.IsType<List<Totira.Bussiness.PropertiesService.DTO.PropertyImage>>(result.CarrouselImages);
            result.CarrouselImages.Count().Equals(0);

        }
    }
}

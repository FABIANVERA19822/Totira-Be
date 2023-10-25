using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.DTO;

namespace Test.PropertiesService.Queries
{
    public class GetLocationsBySearchKeywordQueryHandlerTest
    {
        private readonly Mock<IRepository<Property, string>> _mockGetLocationsBySearchKeywordRepository;

        public GetLocationsBySearchKeywordQueryHandlerTest()
        {
            _mockGetLocationsBySearchKeywordRepository = MockGetLocationsBySearchKeywordRepository.GetLocationsBySearchKeywordRepository();
        }

        [InlineData("Y")]
        [InlineData("York")]
        [InlineData("Ontario")]
        [InlineData("y")]
        [Theory]
        public async Task HandleAsyncTest_OK( string keyword)
        {
            //Arrange
            var handler = new QueryLocationsBySearchKeywordHandler(_mockGetLocationsBySearchKeywordRepository.Object);
            var query = new QueryLocationsBySearchKeyword(keyword);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.NotEmpty(result);
            Assert.True(result.Count>=1);
            Assert.IsType<List<LocationDto>>(result); 
        }

        [Fact]
        public async Task HandleAsyncTest_NoData()
        {
            //Arrange
            var handler = new QueryLocationsBySearchKeywordHandler(_mockGetLocationsBySearchKeywordRepository.Object);
            var query = new QueryLocationsBySearchKeyword(" "); 

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            Assert.Empty(result);
            Assert.True(result.Count == 0);
            Assert.IsType<List<LocationDto>>(result);
        }

        [Fact]
        public async Task HandleAsyncTest_KeyWordAll_ReturnAllData()
        {
            //Arrange
            var handler = new QueryLocationsBySearchKeywordHandler(_mockGetLocationsBySearchKeywordRepository.Object);
            var query = new QueryLocationsBySearchKeyword("All");

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            Assert.True(result.Count > 0);
            Assert.IsType<List<LocationDto>>(result);  
        }

    }
}
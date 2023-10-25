using Moq;
using Test.PropertiesService.RepoMocks;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries; 
using static Totira.Support.Persistance.IRepository; 
namespace Test.PropertiesService.Queries
{
    public class QueryPropertyByMlnumHandlerTest
    {

        private readonly Mock<IRepository<Property, string>> _propertydataRepositoryMock; 
        public QueryPropertyByMlnumHandlerTest()
        { 
            _propertydataRepositoryMock = MockPropertyRepository.GetPropertyRepository("ResidentialProperty"); 
        }

        [InlineData("X5837337")]
        [Theory]
        public async Task HandleAsyncTest_Ok(string data)
        {
            //Arrange
            var handler = new QueryPropertyByMlnumHandler(_propertydataRepositoryMock.Object);
            var query = new QueryPropertyByMlnum(data);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.NotNull(result);
            Assert.IsType<GetPropertyDetailsDto>(result);
        }
       
        [Fact]
        public async Task HandleAsyncTest_NoretrieveData()
        {
            //Arrange
            var handler = new QueryPropertyByMlnumHandler(_propertydataRepositoryMock.Object);
            var query = new QueryPropertyByMlnum(string.Empty);

            //Act
            var result = await handler.HandleAsync(query);

            //Assert

            Assert.True(result.Id == string.Empty);
            Assert.IsType<GetPropertyDetailsDto>(result);
        }
    }
}

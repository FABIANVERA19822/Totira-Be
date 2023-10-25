using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Test.PropertiesService.RepoMocks
{
    public class MockGetLocationsBySearchKeywordRepository
    {
        public static Mock<IRepository<Property, string>> GetLocationsBySearchKeywordRepository()
        {
            var properties = new List<Property>
            {
                new Property
                {
                   Id = "12AB",
                    Bedrooms = 3,
                    Province = "Cairo",
                    ListPrice = 100000

                },
                new Property
                {
                   Id = "123ABC",
                    Bedrooms = 5,
                    Province = "SanFrancisco",
                    ListPrice = 10000

                },
                new Property
                {
                   Id = "1234ABCD",
                    Bedrooms = 4,
                    Province = "Canada",
                    ListPrice = 130000

                },
                 new Property
                {
                   Id = "1234ABCD",
                    Bedrooms = 7,
                    Province = "toronto",
                    ListPrice = 130100

                },
            };

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<Property, string>>();
            #endregion

            #region SetupAllMethods

           



            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Property, bool>>>()))
                        .ReturnsAsync((Expression<Func<Property, bool>> expression) =>
                        properties.Where(expression.Compile()).ToList());

            #endregion


            return mockRepo;

        }

    }
}

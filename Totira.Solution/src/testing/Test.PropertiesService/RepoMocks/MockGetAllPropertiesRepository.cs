
using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.PropertiesService.Domain;
using CrestApps.RetsSdk.Models;
using CrestApps.RetsSdk.Services;
using Org.BouncyCastle.Math.Field;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Test.PropertiesService.RepoMocks;

public class MockGetAllPropertiesRepository
{
    public static Mock<IRepository<Property, string>> GetAllProperties()
    {
        var properties = new List<Property>
            {
                new Property
                {
                    Id = "12AB",
                    Bedrooms = 3,
                    Area="Area1",
                    ListPrice = 100,
                    Address = "Address1",
                    StreetName = "StreetName1",
                    OriginalPrice = 50,
                    ApproxSquareFootage = "ApproxSquareFootage",
                    Washrooms = 2,
                    CreatedOn = DateTime.Now.AddDays(-3),
                },
                new Property
                {
                   Id = "123ABC",
                    Bedrooms = 5,
                    ListPrice = 200,
                                        Address = "Address1",
                    StreetName = "StreetName1",
                    OriginalPrice = 50,
                    ApproxSquareFootage = "ApproxSquareFootage",
                    Washrooms = 2,
                    CreatedOn = DateTime.Now.AddDays(-2),
                },
                new Property
                {
                   Id = "1234ABCD",
                    Bedrooms = 4,
                    ListPrice = 300,
                    Address = "Address1",
                    StreetName = "StreetName1",
                    OriginalPrice = 50,
                    ApproxSquareFootage = "ApproxSquareFootage",
                    Washrooms = 2,
                    CreatedOn = DateTime.Now.AddDays(-1),
                },
                 new Property
                {
                   Id = "1234ABCDE",
                    Bedrooms = 7,
                    ListPrice = 400,
                    Address = "Address1",
                    StreetName = "StreetName1",
                    OriginalPrice = 50,
                    ApproxSquareFootage = "ApproxSquareFootage",
                    Washrooms = 2,
                    CreatedOn = DateTime.Now
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


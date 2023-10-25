
namespace Test.PropertiesService.RepoMocks;

using Moq;
using static Totira.Support.Persistance.IRepository; 
using Totira.Bussiness.PropertiesService.Domain;
using CrestApps.RetsSdk.Models;
using CrestApps.RetsSdk.Services;

public class MockPropertyRepository
{
    public static Mock<IRepository<Property, string>> GetPropertyRepository( string propertyType)
    {
        #region MockingData
        var properties = new List<Property>();

        if (propertyType == "ResidentialProperty")
        { 
            properties.Add(
            new Property
            {
                Id = "C523423",
                Bedrooms = 1,
                Rooms = 2,
                Address= "13 Portneuf Crt",
                Area="Toronto",
                PropertyFeatures = new List<string>()
                  {
                  "",
                  "",
                  "",
                  null,
                  null,
                  null

                  },
                ListPrice = 10000,
                Province = "Ontario",
                PropertyType = "ResidentialProperty",
                residential = new Residential()
                {
                    Acreage = ".50-1.99",
                    LegalDescription = "Pt Lt 11 Con 3 Georgina As In R158621",
                    OtherStructures = new List<string>() { null, null }
                },
                condo = new Condo()
            });
         }
        else
        {
            properties.Add(
            new Property
            {
                Id = "C523421",
                Bedrooms = 1,
                Rooms = 2,
                Address = "13 Portneuf Crt",
                Area = "Toronto",
                PropertyFeatures = new List<string>()
                  {
                  "",
                  "",
                  "",
                  null,
                  null,
                  null

                  },
                ListPrice = 10000,
                Province = "Ontario",
                PropertyType = "CondoProperty",
                residential = new Residential(),
                condo = new Condo()
                {
                    BuildingAmenities = new List<string>()
                    {   "Pool",
                        "Gym",
                        null,
                        null,
                        null,
                        null
                    },
                    Balcony ="Y",
                    PetsPermitted="Y"
                }
            });

        }
        
        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRepository<Property, string>>();
        #endregion

        #region SetupAllMethods

        mockRepo.Setup(r => r.Get(x => true))
                .ReturnsAsync(properties);

        mockRepo.Setup(r => r.Add(It.IsAny<Property>()))
                .Callback<Property>(sa => properties.Add(sa));

        mockRepo.Setup(r => r.Update(It.IsAny<Property>()))
                .Verifiable();

        #endregion
        return mockRepo;
    }

    public static Mock<IRetsClient> GetClientRepository()
    {
        #region MockingData

        RetsResource resource = new RetsResource(){};
       
        var resultSearch = new SearchResult(resource, "ResidentialProperty", "");

        string[] tableColumns = { "ml_num","br","county","lp_dol","addr","area" };

        string[] fields = { "C523423", "1", "Ontario", "10000", "13 Portneuf Crt", "Toronto" };


        SearchResultRow row = new SearchResultRow(tableColumns, fields, "ml_num", "****");

        resultSearch.AddRow(row);

        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRetsClient>();
        #endregion

        #region SetupAllMethods

        mockRepo.Setup(r => r.Search(It.IsAny<SearchRequest>())).ReturnsAsync(resultSearch); 
        #endregion
        return mockRepo;

    }

    public static Mock<IRetsClient> GetClientRepositoryError()
    {
        #region MockingData

        RetsResource resource = new RetsResource() { };

        var resultSearch = new SearchResult(resource, "ResidentialProperty", "");
 
        #endregion

        #region instanciateTheMock
        var mockRepo = new Mock<IRetsClient>();
        #endregion

        #region SetupAllMethods
         
        mockRepo.Setup(r => r.Search(It.IsAny<SearchRequest>())).Throws(new InvalidOperationException("test error", new SystemException()));
        #endregion
        return mockRepo;

    }
}


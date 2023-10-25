
using Moq;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Domain;

namespace Test.PropertiesService.RepoMocks
{
    public class MockGetPropertyCarrouselImagesRepository
    {
        public static Mock<IRepository<PropertyImages, string>> GetPropertyCarrouselImages_WhenPropertyExistsRepository()
        {
            #region MockingData
            var propertiesImages = new List<PropertyImages>
            {
                new PropertyImages
                {
                   Id= "C523423",
                   Propertyimages = new List<PropertyImage>{ new PropertyImage() { Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"), ContentType="image/jpeg", ImageOrder=1, S3KeyName=""} }
                }
            };
            #endregion
            
            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<PropertyImages, string>>();
            #endregion

            #region SetupAllMethods 

            mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<PropertyImages, bool>>>()))
                        .ReturnsAsync((Expression<Func<PropertyImages, bool>> expression) =>
                        propertiesImages.Where(expression.Compile()).ToList());

            #endregion

            return mockRepo;

        }
    }
}

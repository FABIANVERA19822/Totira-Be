using Totira.Bussiness.PropertiesService.DTO;

namespace Test.Services.PropertiesService.Mocks
{
    internal class StaticModels
    {
        public static GetPropertyDetailsDto GetPropertyData()
        {


            return new GetPropertyDetailsDto("C523423", string.Empty, string.Empty, string.Empty, string.Empty, 50, 30, 1, 2, 1, 12, 1,
                "Y", string.Empty, "Y", "Y", "Y", string.Empty, string.Empty, string.Empty, 1, string.Empty, string.Empty, string.Empty,
                "Y", new List<string>(), new List<string>(), 10000, 15000);

           
        }
    }
}

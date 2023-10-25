using Microsoft.Extensions.Configuration;
using Totira.Support.Api.Options;

namespace Totira.Bussiness.PropertiesService.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public readonly string _connectionString = string.Empty;
        public AppConfiguration()
        {        
        }


        public RestClientOptions RestClient() {

            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            

            var root = configurationBuilder.Build();


            var sectionRestClient = root.GetSection("RestClient");


            RestClientOptions options = new RestClientOptions();

            options.ThirdPartyIntegration = sectionRestClient.GetSection("ThirdPartyIntegration").Value ?? string.Empty;

            return options;
        }
        

    }
}

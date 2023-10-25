using Totira.Support.Api.Options;

namespace Totira.Bussiness.UserService.Configuration
{
    public interface IAppConfiguration
    {
        RestClientOptions RestClient();
    }
}
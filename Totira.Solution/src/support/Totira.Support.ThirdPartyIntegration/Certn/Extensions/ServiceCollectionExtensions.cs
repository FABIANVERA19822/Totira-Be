using Microsoft.Extensions.DependencyInjection;
using Totira.Business.Integration.Certn.CommonlHandlers;
using Totira.Business.Integration.Certn.Interfaces;

namespace Totira.Business.Integration.Certn.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCertnLibrary(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICertnHandler, CertnHandler>();
            return serviceCollection;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Totira.Support.CommonLibrary.CommonlHandlers;
using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Support.CommonLibrary.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonLibrary(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEmailHandler, EmailHandler>();
            serviceCollection.AddSingleton<IS3Handler, S3Handler>();
            serviceCollection.AddSingleton<IEncryptionHandler, EncryptionHandler>();
            return serviceCollection;
        }

    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Otp.Configurations;

namespace Totira.Support.Otp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOtp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IOtpHandler, Otphandler>();
            return serviceCollection;
        }
    }
}
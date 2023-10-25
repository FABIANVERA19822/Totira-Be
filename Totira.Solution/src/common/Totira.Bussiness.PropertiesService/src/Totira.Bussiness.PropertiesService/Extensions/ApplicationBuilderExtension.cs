using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Totira.Bussiness.PropertiesService.Commands;
using Totira.Support.EventServiceBus;
namespace Totira.Bussiness.PropertiesService.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public async static Task<IApplicationBuilder> UseMLSEventBus(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IEventBus>();
            //commands
            await bus.SubscribeAsync<CreatePropertyfromRETSCommand>();
            await bus.SubscribeAsync<ImportPropertyImagesToS3Command>();
            return app;
        }
    }
}

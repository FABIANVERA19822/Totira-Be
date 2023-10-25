using CrestApps.RetsSdk.Services;
using Microsoft.Extensions.DependencyInjection;
using Totira.Bussiness.PropertiesService.Commands;
using Totira.Bussiness.PropertiesService.Configuration;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Handlers.Commands;
using Totira.Bussiness.PropertiesService.Handlers.Queries;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Bussiness.PropertiesService.Validators;
using Totira.Support.Application.Extensions;
using Totira.Support.CommonLibrary.CommonlHandlers;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.Persistance.Mongo;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IRetsRequester, RetsWebRequester>();
            services.AddTransient<IRetsSession, RetsSession>();
            services.AddTransient<IRetsClient, RetsClient>();
            services.AddTransient<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IS3Handler, S3Handler>();
            services.AddLogging(); 
 
            //Queries            
            services.AddQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto, QueryPropertyByMlnumHandler>();
            services.AddQueryHandler<QueryProperty, GetPropertyDto, QueryPropertyHandler>();
            services.AddQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>, QueryLocationsBySearchKeywordHandler>();
            //Commands
            services.AddCommandHandler<CreatePropertyfromRETSCommand, CreatePropertyfromRETSCommandHandler, CreatePropertyfromRETSCommandValidator>();
            services.AddCommandHandler<ImportPropertyImagesToS3Command, ImportPropertyImagesToS3CommandHandler, ImportPropertyImagesToS3CommandValidator>();
            services.AddMessageDispatcher();



            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }
    }
}

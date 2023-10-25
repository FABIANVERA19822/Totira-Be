using Microsoft.Extensions.DependencyInjection;
using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Commands.Persona;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Persona;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Certn;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Jira;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Persona;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Queries;
using Totira.Business.ThirdPartyIntegrationService.Queries.Certn;
using Totira.Business.ThirdPartyIntegrationService.Queries.Jira;
using Totira.Business.ThirdPartyIntegrationService.Queries.Persona;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Business.ThirdPartyIntegrationService.Validators.Jira;
using Totira.Business.ThirdPartyIntegrationService.Validators.Persona;
using Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Handlers.Commands.VerifiedProfile;
using Totira.Support.Application.Extensions;
using Totira.Support.Persistance.Mongo;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            // Queries
            services.AddQueryHandler<QueryApplicationByTenantId, GetCertnApplicationDto, QueryApplicationByTenantIdHandler>();
            services.AddQueryHandler<QueryApplication, ListTenantApplicationDto, QueryApplicationHandler>();
            services.AddQueryHandler<QueryPropertyLocationByAddress, GetPropertyLongitudeAndLatitudeDto, QueryPropertyLocationByAddressHandler>();
            services.AddQueryHandler<QueryVerifiedProfileByTenantId, GetTenantVerifiedProfileDto, QueryVerifiedProfileByTenantIdHandler>();
            services.AddQueryHandler<QueryVerifiedProfile, ListTenantVerifiedProfileDto, QueryVerifiedProfileHandler>();
            services.AddQueryHandler<QueryPersonaInquiryByTenantId, GetPersonaApplicationDto, QueryPersonaInquiryByTenantIdHandler>();
            services.AddQueryHandler<QueryTenantApplicationById, TenantApplicationDto, QueryTenantApplicationByIdHandler>();
            services.AddQueryHandler<QueryEmailConfirmationByTenantId, GetTenantVerifiedProfileDto, QueryEmailConfirmationByTenantIdHandler>();
            services.AddQueryHandler<QueryJiraTicketEmployementByTenantId, GetJiraEmployementTicketDto, QueryJiraTicketEmployementByTenantIdHandler>();

            // Commands
            services.AddCommandHandler<ApplySoftCheckCommand, ApplySoftCheckCommandHandler>();
            services.AddCommandHandler<CreateTenantVerifiedProfileCommand, CreateTenantVerifiedProfileCommandHandler>();
            services.AddCommandHandler<UpdateTenantVerifiedProfileCommand, UpdateTenantVerifiedProfileCommandHandler>();
            services.AddCommandHandler<TenantPersonaValidationCommand, TenantPersonaValidationCommandHandler, TenantPersonaValidationCommandValidator>();
            services.AddCommandHandler<TenantEmployeeAndIncomeTicketJiraCommand, TenantCreateJiraTicketEmployeeIncomeCommandHandler, TenantEmployeeAndIncomeTicketJiraCommandValidator>();
            services.AddCommandHandler<UpdateTenantEmployeeAndIncomeTicketJiraCommand, TenantUpdateJiraTicketEmployeeIncomeCommandHandler, UpdateTenantEmployeeAndIncomeTicketJiraCommandValidator>();
            services.AddCommandHandler<CreateProfileInterestJiraTicketCommand, CreateProfileInterestJiraTicketCommandHandler, CreateProfileInterestJiraTicketCommandValidator>();
            services.AddCommandHandler<CreateGroupProfileInterestJiraticketCommand, CreateGroupProfileInterestJiraticketCommandHandler, CreateGroupProfileInterestJiraticketCommandValidator>();

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


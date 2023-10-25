using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Commands.Persona;
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Support.EventServiceBus;

namespace Totira.Business.ThirdPartyIntegrationService.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public async static Task<IApplicationBuilder> UseThirdPartyIntegrationEventBus(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IEventBus>();

            await bus.SubscribeAsync<ApplySoftCheckCommand>();
            await bus.SubscribeAsync<CreateTenantVerifiedProfileCommand>();
            await bus.SubscribeAsync<TenantPersonaValidationCommand>();
            await bus.SubscribeAsync<TenantEmployeeAndIncomeTicketJiraCommand>();
            await bus.SubscribeAsync<UpdateTenantEmployeeAndIncomeTicketJiraCommand>();
            await bus.SubscribeAsync<CreateProfileInterestJiraTicketCommand>();
            await bus.SubscribeAsync<CreateGroupProfileInterestJiraticketCommand>();
            await bus.SubscribeAsync<UpdateTenantVerifiedProfileCommand>();
            await bus.SubscribeAsync<ApplyCertnGroupCommand>();
            await bus.SubscribeAsync<ApplyJiraGroupCommand>();

            return app;
        }
    }
}


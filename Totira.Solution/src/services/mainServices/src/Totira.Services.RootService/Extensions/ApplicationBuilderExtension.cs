using Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira;
using Totira.Services.RootService.Events.UserService;
using Totira.Support.EventServiceBus;

namespace Totira.Services.RootService.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IEventBus>();

            #region UserService

            bus.SubscribeAsync<AcceptTermsAndConditionsCreatedEvent>();
            bus.SubscribeAsync<TenantAcquaintanceReferralUpdateEvent>();
            bus.SubscribeAsync<TenantApplicationDetailsCreatedEvent>();
            bus.SubscribeAsync<TenantApplicationDetailsUpdatedEvent>();
            bus.SubscribeAsync<TenantApplicationRequestCoapplicantDeletedEvent>();
            bus.SubscribeAsync<TenantApplicationRequestCreatedEvent>();
            bus.SubscribeAsync<TenantApplicationRequestDeletedEvent>();
            bus.SubscribeAsync<TenantApplicationRequestGuarantorDeletedEvent>();
            bus.SubscribeAsync<TenantApplicationRequestUpdatedEvent>();
            bus.SubscribeAsync<TenantBasicInformationCreatedEvent>();
            bus.SubscribeAsync<TenantBasicInformationUpdatedEvent>();
            bus.SubscribeAsync<TenantContactInformationCreatedEvent>();
            bus.SubscribeAsync<TenantContactInformationUpdatedEvent>();
            bus.SubscribeAsync<TenantEmployeeIncomeFileDeletedEvent>();
            bus.SubscribeAsync<TenantEmployeeIncomeIdDeletedEvent>();
            bus.SubscribeAsync<TenantEmployeeIncomesCreatedEvent>();
            bus.SubscribeAsync<TenantEmploymentReferenceCreatedEvent>();
            bus.SubscribeAsync<TenantEmploymentReferenceUpdatedEvent>();
            bus.SubscribeAsync<TenantFeedbackViaLandlordCreatedEvent>();
            bus.SubscribeAsync<TenantPersonalInformationCreatedEvent>();
            bus.SubscribeAsync<TenantPersonalInformationUpdatedEvent>();
            bus.SubscribeAsync<TenantProfileImageCreatedEvent>();
            bus.SubscribeAsync<TenantProfileImageUpdatedEvent>();
            bus.SubscribeAsync<TenantRentalHistoriesUpdatedEvent>();
            bus.SubscribeAsync<TenantShareProfileCreatedEvent>();
            bus.SubscribeAsync<TenantEmployeeIncomesUpdatedEvent>();
            bus.SubscribeAsync<TenantStudentFinancialDetailCreatedEvent>();
            bus.SubscribeAsync<TenantStudentFinancialDetailUpdatedEvent>();
            bus.SubscribeAsync<TenantStudentDetailDeletedEvent>();
            bus.SubscribeAsync<UserCreatedEvent>();

            #endregion

            #region ThirdpartyService

            bus.SubscribeAsync<TenantJiraTicketEmployeeIncomeUpdatedEvent>();
            bus.SubscribeAsync<TenantPersonaValidationCreatedEvent>();
            bus.SubscribeAsync<TenantPersonaValidationUpdatedEvent>();


            #endregion



            return app;
        }
    }
}


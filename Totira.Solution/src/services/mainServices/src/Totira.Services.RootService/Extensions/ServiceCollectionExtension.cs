using Totira.Support.Application.Events;
using Totira.Support.NotificationHub;
using Totira.Support.Application.Extensions;
using Totira.Services.RootService.Handlers;
using Totira.Services.RootService.Events.UserService;
using Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira;
using Totira.Services.RootService.Events.Landlord.CreatedEvents;
using Totira.Services.RootService.Events;
using Totira.Services.RootService.Events.Landlord.UpdatedEvents;

namespace Totira.Services.RootService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            return services.AddNotificationHandler<AcceptTermsAndConditionsCreatedEvent>()
                           .AddNotificationHandler<TenantAcquaintanceReferralUpdateEvent>()
                           .AddNotificationHandler<TenantApplicationDetailsCreatedEvent>()
                           .AddNotificationHandler<TenantApplicationDetailsUpdatedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestCoapplicantDeletedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestCreatedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestDeletedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestGuarantorDeletedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestUpdatedEvent>()
                           .AddNotificationHandler<TenantBasicInformationCreatedEvent>()
                           .AddNotificationHandler<TenantBasicInformationUpdatedEvent>()
                           .AddNotificationHandler<TenantContactInformationCreatedEvent>()
                           .AddNotificationHandler<TenantContactInformationUpdatedEvent>()
                           .AddNotificationHandler<TenantEmployeeIncomeFileDeletedEvent>()
                           .AddNotificationHandler<TenantEmployeeIncomeIdDeletedEvent>()
                           .AddNotificationHandler<TenantEmployeeIncomesCreatedEvent>()
                           .AddNotificationHandler<TenantEmploymentReferenceCreatedEvent>()
                           .AddNotificationHandler<TenantEmploymentReferenceUpdatedEvent>()
                           .AddNotificationHandler<TenantFeedbackViaLandlordCreatedEvent>()
                           .AddNotificationHandler<TenantPersonalInformationCreatedEvent>()
                           .AddNotificationHandler<TenantPersonalInformationUpdatedEvent>()
                           .AddNotificationHandler<TenantProfileImageCreatedEvent>()
                           .AddNotificationHandler<TenantProfileImageUpdatedEvent>()
                           .AddNotificationHandler<TenantRentalHistoriesUpdatedEvent>()
                           .AddNotificationHandler<TenantShareProfileCreatedEvent>()
                           .AddNotificationHandler<UserCreatedEvent>()
                           .AddNotificationHandler<TenantJiraTicketEmployeeIncomeUpdatedEvent>()
                           .AddNotificationHandler<TenantPersonaValidationCreatedEvent>()
                           .AddNotificationHandler<TenantPersonaValidationUpdatedEvent>()
                           .AddNotificationHandler<TenantEmployeeIncomesUpdatedEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailCreatedEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailUpdatedEvent>()
                           .AddNotificationHandler<TenantStudentDetailDeletedEvent>()
                           .AddNotificationHandler<TenantEmployeeIncomesUpdatedEvent>()
                           .AddMessageDispatcher()
                           .AddNotificationHandler<TenantPersonaValidationUpdatedEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailCreatedEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailUpdatedEvent>()
                           .AddNotificationHandler<TenantStudentDetailDeletedEvent>()
                           .AddNotificationHandler<ApplicationRequestInvitationResponseEvent>()
                           .AddNotificationHandler<TenantApplicationRequestSendEmailsUpdatedEvent>()
                           .AddNotificationHandler<TenantApplicationTypeCreatedEvent>()
                           .AddNotificationHandler<TenantApplicationTypeUpdatedEvent>()
                           .AddNotificationHandler<TenantCosignersLeaveFromGroupApplicationDeletedEvent>()
                           .AddNotificationHandler<TenantCurrentJobStatusCreatedEvent>()
                           .AddNotificationHandler<TenantCurrentJobStatusUpdatedEvent>()
                           .AddNotificationHandler<TenantGroupApplicationShareProfileCreatedEvent>()
                           .AddNotificationHandler<TenantGroupVerificationDoneEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailFileDeletedEvent>()
                           .AddNotificationHandler<TenantUnacceptedApplicationRequestDeletedCommandEvent>()
                           .AddNotificationHandler<LandlordBasicInformationCreatedEvent>()
                           .AddNotificationHandler<LandlordIdentityCreatedEvent>()
                           .AddNotificationHandler<LandlordPropertyClaimsCreatedEvent>()
                           .AddNotificationHandler<TenantApplicationRequestSendEmailsUpdatedEvent>()
                           .AddNotificationHandler<TenantApplicationTypeCreatedEvent>()
                           .AddNotificationHandler<TenantApplicationTypeUpdatedEvent>()
                           .AddNotificationHandler<TenantCosignersLeaveFromGroupApplicationDeletedEvent>()
                           .AddNotificationHandler<TenantCurrentJobStatusCreatedEvent>()
                           .AddNotificationHandler<TenantCurrentJobStatusUpdatedEvent>()
                           .AddNotificationHandler<TenantGroupApplicationShareProfileCreatedEvent>()
                           .AddNotificationHandler<TenantGroupVerificationDoneEvent>()
                           .AddNotificationHandler<TenantStudentFinancialDetailFileDeletedEvent>()
                           .AddNotificationHandler<TenantUnacceptedApplicationRequestDeletedCommandEvent>()
                           .AddNotificationHandler<ApplicationRequestApprovedEvent>()
                           .AddNotificationHandler<ApplicationRequestRejectedEvent>()
                           .AddNotificationHandler<CreateLandlordPropertyClaimsJiraTicketEvent>()
                           .AddNotificationHandler<ClaimJiraTicketCreationUpdatedEvent>()
                           .AddNotificationHandler<PropertyApplicationStatusCanceledEvent>()
                           .AddMessageDispatcher();
        }

        public static IServiceCollection AddNotificationHandler<TEvent>(this IServiceCollection services)
           where TEvent : IEvent, INotification
        {
            return services.AddEventHandler<TEvent, NotificationHandler<TEvent>>();
        }
    }
}

﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create;
using Totira.Support.EventServiceBus;

namespace Totira.Bussiness.UserService.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> UseEventBus(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetRequiredService<IEventBus>();
            //
            await bus.SubscribeAsync<CreateTenantBasicInformationCommand>();
            await bus.SubscribeAsync<UpdateTenantBasicInformationCommand>();
            await bus.SubscribeAsync<CreateTenantProfileImageCommand>();
            await bus.SubscribeAsync<CreateTenantApplicationDetailsCommand>();
            await bus.SubscribeAsync<UpdateTenantApplicationDetailsCommand>();
            await bus.SubscribeAsync<CreateTenantAcquaintanceReferralCommand>();
            await bus.SubscribeAsync<UpdateTenantAcquaintanceReferralCommand>();
            await bus.SubscribeAsync<CreateTenantFeedbackViaLandlordCommand>();
            await bus.SubscribeAsync<CreateTenantRentalHistoriesCommand>();
            await bus.SubscribeAsync<UpdateTenantRentalHistoriesCommand>();
            await bus.SubscribeAsync<CreateAcquaintanceReferralFormInfoCommand>();
            await bus.SubscribeAsync<CreateTenantEmployeeIncomesCommand>();
            await bus.SubscribeAsync<UpdateTenantEmployeeIncomesCommand>();
            await bus.SubscribeAsync<DeleteTenantEmployeeIncomeFileCommand>();
            await bus.SubscribeAsync<CreateTenantContactInformationCommand>();
            await bus.SubscribeAsync<UpdateTenantContactInformationCommand>();
            await bus.SubscribeAsync<UpdateTenantProfileImageCommand>();
            await bus.SubscribeAsync<CreateTenantEmploymentReferenceCommand>();
            await bus.SubscribeAsync<UpdateTenantEmploymentReferenceCommand>();
            await bus.SubscribeAsync<UpdateTenantAcquaintanceReferralReactivateCommand>();
            await bus.SubscribeAsync<UpdateTenantRentalHistoriesReactivateCommand>();
            await bus.SubscribeAsync<CreateTenantShareProfileCommand>();
            await bus.SubscribeAsync<AcceptTermsAndConditionsCommand>();
            await bus.SubscribeAsync<ApplicationRequestInvitationResponseCommand>();
            await bus.SubscribeAsync<DeleteTenantEmployeeIncomeIdCommand>();
            await bus.SubscribeAsync<CreateTenantStudentFinancialDetailCommand>();
            await bus.SubscribeAsync<UpdateTenantStudentFinancialDetailCommand>();
            await bus.SubscribeAsync<DeleteTenantStudentFinancialDetailFileCommand>();
            await bus.SubscribeAsync<DeleteTenantApplicationRequestCommand>();
            await bus.SubscribeAsync<UpdateTenantApplicationRequestCommand>();
            await bus.SubscribeAsync<CreateTenantApplicationRequestCommand>();
            await bus.SubscribeAsync<UpdateTenantShareProfileTermsCommand>();
            await bus.SubscribeAsync<CreateTenantApplicationTypeCommand>();
            await bus.SubscribeAsync<UpdateTenantApplicationTypeCommand>();
            await bus.SubscribeAsync<CreateGroupApplicationShareProfileCommand>();
            await bus.SubscribeAsync<DeleteTenantCoSignerFromGroupApplicationProfileCommand>();
            await bus.SubscribeAsync<DeleteTenantStudentDetailCommand>();
            await bus.SubscribeAsync<DeleteTenantUnacceptedApplicationRequestCommand>();
            await bus.SubscribeAsync<CreateTenantCurrentJobStatusCommand>();
            await bus.SubscribeAsync<UpdateTenantCurrentJobStatusCommand>();
            await bus.SubscribeAsync<UpdateApplicationRequestInvitationCommand>();
            await bus.SubscribeAsync<VerifyTenantGroupCommand>();
            await bus.SubscribeAsync<CreatePropertyToApplyCommand>();
            await bus.SubscribeAsync<CreateTenantContactLandlordCommand>();

            #region Landlord

            await bus.SubscribeAsync<CreateLandlordBasicInfoCommand>();
            await bus.SubscribeAsync<CreateLandlordIdentityCommand>();
            await bus.SubscribeAsync<CreatePropertyClaimsFromLandlordCommand>();
            await bus.SubscribeAsync<ApproveApplicationRequestCommand>();
            await bus.SubscribeAsync<RejectApplicationRequestCommand>();
            await bus.SubscribeAsync<UpdateClaimJiraTicketCreationCommand>();
            await bus.SubscribeAsync<UpdatePropertyClaimsFromJiraTicketCommand>();
            await bus.SubscribeAsync<UpdateClaimJiraTicketResultCommand>();
            await bus.SubscribeAsync<CancelStatusPropertyApplicationCommand>();
            await bus.SubscribeAsync<CreateLandlordPropertyDisplayCommand>();

            #endregion Landlord

            return app;
        }
    }
}
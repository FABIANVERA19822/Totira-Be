using Microsoft.Extensions.DependencyInjection;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Configuration;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.DTO.GroupTenant;
using Totira.Bussiness.UserService.DTO.GroupTenant.Certn;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Handlers.Queries.Files;
using Totira.Bussiness.UserService.Handlers.Queries.Landlord;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Queries.Files;
using Totira.Bussiness.UserService.Queries.Group;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Bussiness.UserService.Repositories;
using Totira.Bussiness.UserService.Validators;
using Totira.Bussiness.UserService.Validators.Landlord;
using Totira.Bussiness.UserService.Validators.Landlord.Created;
using Totira.Bussiness.UserService.Validators.Landlord.Updated;
using Totira.Support.Application.Extensions;
using Totira.Support.Otp.DTO;
using Totira.Support.Persistance.Mongo;
using Totira.Support.TransactionalOutbox.Extensions;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Common
            services.AddTransient<ICommonFunctions, CommonFunctions>();
            services.AddTransient<IAppConfiguration, AppConfiguration>();

            //Queries
            services.AddQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto, GetTenantBasicInformationByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto, GetTenantProfileImageByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto, GetTenantApplicationDetailsByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto, GetTenantAcquaintanceReferralByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantAcquaintanceReferralEmailsById, GetTenantAquaintanceReferralEmailsDto, GetTenantAcquaintanceReferralEmailsByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto, GetTenantFeedbackViaLandlordByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto, GetTenantRentalHistoriesByIdQueryHandler>();
            services.AddQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto, GetAcquaintanceReferralFormInfoByReferralIdQueryHandler>();
            services.AddQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto, GetTenantEmployeeIncomesByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantEmployeeIncomeById, GetTenantEmployeeIncomeDto, GetTenantEmployeeIncomeByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto, GetTenantContactInformationByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantEmploymentReferenceById, GetTenantEmploymentReferenceDto, GetTenantEmploymentReferenceByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantInformationForCertnApplicationById, GetTenantInformationForCertnApplicationDto, GetTenantInformationForCertnApplicationByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>, GetTenantProfileProgressQueryHandler>();
            services.AddQueryHandler<QueryTenantProfileFunnelByTenantId, Dictionary<string, bool>, GetTenantProfileFunnelByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantProfileSummaryById, GetTenantProfileSummaryDto, GetTenantProfileSummaryByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto, CheckTenantShareProfileCodeAndEmailHandler>();
            services.AddQueryHandler<QueryTenantGroupShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto, CheckTenantGroupShareProfileCodeAndEmailHandler>();
            services.AddQueryHandler<QueryValidateLinkOtp, ValidateLinkOtpDto, ValidateLinkOtpQueryHandler>();
            services.AddQueryHandler<QueryValidateOtp, ValidateOtpDto, ValidateOtpQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupApplicationSummaryById, GetTenantGroupApplicationSummaryDto, GetTenantGroupApplicationSummaryByIdQueryHandler>();
            services.AddQueryHandler<QueryVerificationsProcessStatusById, string, GetVerificationsProcessStatusByIdQueryHandler>();
            services.AddQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto, GetAllApplicationRequestByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto, GetApplicationRequestByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTermsAndConditionsByApplicationRequestId, bool?, GetTermsAndConditionsByApplicationRequestIdQueryHandler>();
            services.AddQueryHandler<QueryTenantApplicationRoleByTenantId, string, GetTenantApplicationRoleQueryHandler>();
            services.AddQueryHandler<QueryVerificationsProcessStatusById, string, GetVerificationsProcessStatusByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto, GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler>();
            services.AddQueryHandler<QueryDownloadTenantIncomeFile, DownloadFileDto, DownloadTenantIncomeFileQueryHandler>();
            services.AddQueryHandler<QueryTenantShareProfileByTenantId, GetTenantShareProfileDto, GetTenantShareProfileByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto, CheckUserIsExistByEmailHandler>();
            services.AddQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto, GetTenantVerifiedProfileByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupInventeesByTenantId, List<TenantGroupApplicationProfile>, GetTenantGroupInventeesByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupInventees, ListTenantGroupApplicationProfile, GetTenantGroupInventeesHandler>();
            services.AddQueryHandler<QueryApplicationTypeByTenantId, GetTenantApplicationTypeByDto, GetTenantApplicationTypeByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupEmailConfirmationByTenantId, List<TenantGroupApplicationProfile>, GetTenantGroupEmailConfirmationByTenantIdHandler>();
            services.AddQueryHandler<QueryTermsAndConditionsByTenantId, GetTermsAndConditionsByTenantIdDto, GetTermsAndConditionsByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantStudentDetailById, GetTenantStudentDetailByIdDto, GetTenantStudentDetailByIdQueryHandler>();
            services.AddQueryHandler<QueryCurrentJobStatusByTenantId, GetTenantCurrentJobStatusByDto, GetTenantCurrentJobStatusByIdQueryHandler>();
            services.AddQueryHandler<QueryApplicationRequestbyInvitationId, GetApplicationRequestbyInvitationDto, GetApplicationRequestbyInvitationIdQueryHandler>();
            services.AddQueryHandler<QueryInvitationsByApplicationRequestById, GetAllInvitationsToJoinByApplicationRequestDto, GetAllInvitationbyApplicationRequestIdQueryHandler>();
            services.AddQueryHandler<QueryTenantApplicationListPageById, GetTenantApplicationListPageDto, GetTenantApplicationListPageByIdQueryHandler>();

            services.AddQueryHandler<QueryGetGroupApplicantsInfoByTenantId, GetGroupApplicantsInfoByTenantIdDto, GetGroupApplicantsInfoByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryGetGroupVerifiedProfilebyTenantId, GetTenantGroupVerifiedProfileDto, GetTenantGroupVerifiedProfileQueryHandler>();
            services.AddQueryHandler<QueryGetGroupDashboardProfileByTenantId, GetTenantGroupDashboardProfileDto, GetTenantGroupDashboardProfileQueryHandler>();
            services.AddQueryHandler<QueryTenantPropertyApplication, GetPropertyAppliedDto, GetTenantPropertyApplicationQueryHandler>();

            //Commands            ;
            services.AddCommandHandler<CreateTenantBasicInformationCommand, CreateTenantBasicInformationCommandHandler, CreateTenantBasicInformationCommandValidator>();
            services.AddCommandHandler<CreateTenantProfileImageCommand, CreateTenantProfileImageCommandHandler, CreateTenantProfileImageCommandValidator>();
            services.AddCommandHandler<UpdateTenantBasicInformationCommand, UpdateTenantBasicInformationCommandHandler, UpdateTenantBasicInformationCommandValidator>();
            services.AddCommandHandler<UpdateTenantProfileImageCommand, UpdateTenantProfileImageCommandHandler, UpdateTenantProfileImageCommandValidator>();
            services.AddCommandHandler<CreateTenantApplicationDetailsCommand, CreateTenantApplicationDetailsCommandHandler, CreateTenantApplicationDetailsCommandValidator>();
            services.AddCommandHandler<UpdateTenantApplicationDetailsCommand, UpdateTenantApplicationDetailsCommandHandler, UpdateTenantApplicationDetailsCommandValidator>();
            services.AddCommandHandler<CreateTenantAcquaintanceReferralCommand, CreateTenantAcquaintanceReferralCommandHandler, CreateTenantAcquaintanceReferralCommandValidator>();
            services.AddCommandHandler<UpdateTenantAcquaintanceReferralCommand, UpdateTenantAcquaintanceReferralCommandHandler, UpdateTenantAcquaintanceReferralCommandValidator>();
            services.AddCommandHandler<CreateTenantRentalHistoriesCommand, CreateTenantRentalHistoriesCommandHandler, CreateTenantRentalHistoriesCommandValidator>();
            services.AddCommandHandler<UpdateTenantRentalHistoriesCommand, UpdateTenantRentalHistoriesCommandHandler, UpdateTenantRentalHistoriesCommandValidator>();
            services.AddCommandHandler<CreateAcquaintanceReferralFormInfoCommand, CreateAcquaintanceReferralFormInfoCommandHandler, CreateAcquaintanceReferralFormInfoCommandValidator>();
            services.AddCommandHandler<CreateTenantFeedbackViaLandlordCommand, CreateTenantFeedbackViaLandlordCommandHandler, CreateTenantFeedbackViaLandlordCommandValidator>();
            services.AddCommandHandler<CreateTenantEmployeeIncomesCommand, CreateTenantEmployeeIncomesCommandHandler, CreateTenantEmployeeIncomesCommandValidator>();
            services.AddCommandHandler<UpdateTenantEmployeeIncomesCommand, UpdateTenantEmployeeIncomesCommandHandler, UpdateTenantEmployeeIncomesCommandValidator>();
            services.AddCommandHandler<DeleteTenantEmployeeIncomeFileCommand, DeleteTenantEmployeeIncomeFileCommandHandler, DeleteTenantEmployeeIncomeFileValidator>();
            services.AddCommandHandler<CreateTenantContactInformationCommand, CreateTenantContactInformationCommandHandler, CreateTenantContactInformationCommandValidator>();
            services.AddCommandHandler<UpdateTenantContactInformationCommand, UpdateTenantContactInformationCommandHandler, UpdateTenantContactInformationCommandValidator>();
            services.AddCommandHandler<CreateTenantEmploymentReferenceCommand, CreateTenantEmploymentReferenceCommandHandler, CreateTenantEmploymentReferenceCommandValidator>();
            services.AddCommandHandler<UpdateTenantEmploymentReferenceCommand, UpdateTenantEmploymentReferenceCommandHandler, UpdateTenantEmploymentReferenceCommandValidator>();
            services.AddCommandHandler<UpdateTenantAcquaintanceReferralReactivateCommand, UpdateTenantAcquaintanceReferralReactivateCommandHandler, UpdateTenantAcquaintanceReferralReactivateValidator>();
            services.AddCommandHandler<UpdateTenantRentalHistoriesReactivateCommand, UpdateTenantRentalHistoriesReactivateCommandHandler, UpdateTenantRentalHistoriesReactivateCommandValidator>();
            services.AddCommandHandler<CreateTenantShareProfileCommand, CreateTenantShareProfileCommandHandler, CreateTenantShareProfileCommandValidator>();
            services.AddCommandHandler<AcceptTermsAndConditionsCommand, AcceptTermsAndConditionsCommandHandler, AcceptTermsAndConditionsCommandValidator>();
            services.AddCommandHandler<UpdateTenantShareProfileTermsCommand, UpdateTenantShareProfileTermsHandler, UpdateTenantShareProfileTermsValidator>();
            services.AddCommandHandler<DeleteTenantEmployeeIncomeIdCommand, DeleteTenantEmployeeIncomeIdCommandHandler, DeleteTenantEmployeeIncomeIdValidator>();
            services.AddCommandHandler<CreateTenantStudentFinancialDetailCommand, CreateTenantStudentFinancialDetailCommandHandler, CreateTenantStudentFinancialDetailCommandValidator>();
            services.AddCommandHandler<UpdateTenantStudentFinancialDetailCommand, UpdateTenantStudentFinancialDetailCommandHandler, UpdateTenantStudentFinancialDetailCommandValidator>();
            services.AddCommandHandler<DeleteTenantStudentFinancialDetailFileCommand, DeleteTenantStudentFinancialDetailFileCommandHandler, DeleteTenantStudentFinancialDetailFileCommandValidator>();
            services.AddCommandHandler<ApplicationRequestInvitationResponseCommand, ApplicationRequestInvitationResponseCommandHandler, ApplicationRequestInvitationResponseValidator>();

            services.AddCommandHandler<CreateTenantApplicationRequestCommand, CreateTenantApplicationRequestCommandHandler, CreateTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<UpdateTenantApplicationRequestCommand, UpdateTenantApplicationRequestCommandHandler, UpdateTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestCommand, DeleteTenantApplicationRequestCommandHandler, DeleteTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestGuarantorCommand, DeleteTenantApplicationRequestGuarantorCommandHandler, DeleteTenantApplicationRequestGuarantorCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestCoapplicantCommand, DeleteTenantApplicationRequestCoapplicantCommandHandler, DeleteTenantApplicationRequestCoapplicantCommandValidator>();
            services.AddCommandHandler<UpdateGroupApplicationCommand, UpdateGroupApplicationCommandHandler, UpdateGroupApplicationCommandValidator>();
            services.AddCommandHandler<CreateTenantApplicationTypeCommand, CreateTenantApplicationTypeCommandHandler, CreateTenantApplicationTypeCommandValidator>();
            services.AddCommandHandler<UpdateTenantApplicationTypeCommand, UpdateTenantApplicationTypeCommandHandler, UpdateTenantApplicationTypeCommandValidator>();
            services.AddCommandHandler<CreateGroupApplicationShareProfileCommand, CreateGroupApplicationShareProfileCommandHandler, CreateGroupApplicationShareProfileCommandValidator>();
            services.AddCommandHandler<DeleteTenantCoSignerFromGroupApplicationProfileCommand, DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler, DeleteTenantCoSignerFromGroupApplicationProfileCommandValidator>();
            services.AddCommandHandler<DeleteTenantStudentDetailCommand, DeleteTenantStudentDetailCommandHandler, DeleteTenantStudentDetailCommandValidator>();
            services.AddCommandHandler<CreateTenantCurrentJobStatusCommand, CreateTenantCurrentJobStatusCommandHandler, CreateTenantCurrentJobStatusCommandValidator>();
            services.AddCommandHandler<UpdateTenantCurrentJobStatusCommand, UpdateTenantCurrentJobStatusCommandHandler, UpdateTenantCurrentJobStatusCommandValidator>();

            services.AddCommandHandler<UpdateApplicationRequestInvitationCommand, UpdateApplicationRequestInvitationCommandHandler, UpdateApplicationRequestInvitationCommandValidator>();

            services.AddCommandHandler<DeleteTenantUnacceptedApplicationRequestCommand, DeleteTenantUnacceptedApplicationRequestCommandHandler, DeleteTenantUnacceptedApplicationRequestCommandValidator>();

            services.AddCommandHandler<VerifyTenantGroupCommand, VerifyTenantGroupCommandHandler>();
            services.AddCommandHandler<CreatePropertyToApplyCommand, CreateTenantPropertyApplicationCommandHandler, CreateTenantPropertyApplicationCommandValidator>();
            services.AddCommandHandler<CreateTenantContactLandlordCommand, CreateTenantContactLandlordCommandHandler, CreateTenantContactLandlordCommandValidator>();

            ///// Landlord Command & Querys /////

            #region Landlord Commands

            services.AddCommandHandler<CreateLandlordBasicInfoCommand, CreateLandlordBasicInfoHandler, CreatedLandlordBasicInfoValidator>();
            services.AddCommandHandler<CreateLandlordIdentityCommand, CreateLandlordIdentityHandler, CreatedLandlordIdentityValidator>();
            services.AddCommandHandler<CreatePropertyClaimsFromLandlordCommand, CreatePropertyClaimsFromLandlordHandler, CreatePropertyClaimsFromLandlordValidator>();
            services.AddCommandHandler<ApproveApplicationRequestCommand, ApproveApplicationRequestCommandHandler, ApproveApplicationRequestCommandValidator>();
            services.AddCommandHandler<RejectApplicationRequestCommand, RejectApplicationRequestCommandHandler, RejectApplicationRequestCommandValidator>();
            services.AddCommandHandler<UpdateClaimJiraTicketCreationCommand, UpdateClaimJiraTicketCreationHandler, UpdateClaimJiraTicketCreationValidator>();
            services.AddCommandHandler<UpdateClaimJiraTicketResultCommand, UpdateClaimJiraTicketResultHandler, UpdateClaimJiraTicketResultValidator>();
            services.AddCommandHandler<UpdatePropertyClaimsFromJiraTicketCommand, UpdatePropertyClaimsFromJiraTicketCommandHandler, UpdatePropertyClaimsFromJiraTicketCommandValidator>();
            services.AddCommandHandler<CancelStatusPropertyApplicationCommand, CancelStatusPropertyApplicationCommandHandler, CancelStatusPropertyApplicationCommandValidator>();
            services.AddCommandHandler<CreateLandlordPropertyDisplayCommand, CreateLandlordPropertyDisplayHandler, CreateLandlordPropertyDisplayValidator>();

            #endregion Landlord Commands

            #region Landlord Querys

            services.AddQueryHandler<QueryLandlordBasicInformationById, GetLandlordBasicInformationDto, GetLandlordBasicInformationByIdQueryHandler>();
            services.AddQueryHandler<QueryLandlordIdentityInformationById, GetLandlordIdentityInformationDto, GetLandlordIdentityInformationByIdQueryHandler>();
            services.AddQueryHandler<QueryDownloadLandlordIdentityFile, DownloadFileDto, DownloadLandlordIdentityFileQueryHandler>();
            services.AddQueryHandler<QueryDownloadLandlordClaimFile, DownloadFileDto, DownloadLandlordClaimFileQueryHandler>();
            services.AddQueryHandler<QueryPropertyClaimsDisplayByLandlordId, GetLandlordClaimsDisplayDto, GetPropertyClaimsDisplayByLandlordIdQueryHandler>();
            services.AddQueryHandler<QueryPendingClaimsByLandlordId, IEnumerable<GetPendingLandlordClaimsDto>, GetPendingClaimsByLandlordIdQueryHandler>();
            services.AddQueryHandler<QueryLandlordApplicationsListPageByLandlordId, GetApplicationListPageDto, GetLandlordApplicationListPageByLandlordIdQueryHandler>();
            services.AddQueryHandler<QueryPropertyApplicationDetail, PropertyApplicationRequestDetailDto, GetPropertyApplicationRequestDetailQueryHandler>();
            services.AddQueryHandler<QueryLandlordPropertiesDisplayByLandlordId, GetLandlordPropertiesDisplayDto, GetLandlordPropertiesDisplayQueryHandler>();

            #endregion Landlord Querys

            services.AddMessageDispatcher();
            services.AddContextFactory();
            services.AddTransactionalOutbox<MessageOutboxRepository>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }
    }
}
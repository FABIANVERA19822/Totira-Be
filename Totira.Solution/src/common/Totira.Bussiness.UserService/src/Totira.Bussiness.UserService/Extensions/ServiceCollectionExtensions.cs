using Microsoft.Extensions.DependencyInjection;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Configuration;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Repositories;
using Totira.Bussiness.UserService.Validators;
using Totira.Support.Application.Extensions;
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
            services.AddQueryHandler<QueryTenantGroupApplicationSummaryById, GetTenantGroupApplicationSummaryDto, GetTenantGroupApplicationSummaryByIdQueryHandler>();
            services.AddQueryHandler<QueryVerificationsProcessStatusById, string, GetVerificationsProcessStatusByIdQueryHandler>();
            services.AddQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto, GetAllApplicationRequestByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto, GetApplicationRequestByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryVerificationsProcessStatusById, string, GetVerificationsProcessStatusByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto, GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler>();
            services.AddQueryHandler<QueryDownloadTenantIncomeFile, DownloadTenantFileDto, DownloadTenantIncomeFileQueryHandler>();
            services.AddQueryHandler<QueryTenantShareProfileByTenantId, GetTenantShareProfileDto, GetTenantShareProfileByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto, CheckUserIsExistByEmailHandler>();
            services.AddQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto, GetTenantVerifiedProfileByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupInventeesByTenantId, List<TenantGroupApplicationProfile>, GetTenantGroupInventeesByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupInventees, ListTenantGroupApplicationProfile, GetTenantGroupInventeesHandler>();
            services.AddQueryHandler<QueryApplicationTypeByTenantId, GetTenantApplicationTypeByDto, GetTenantApplicationTypeByIdQueryHandler>();
            services.AddQueryHandler<QueryTenantGroupEmailConfirmationByTenantId, List<TenantGroupApplicationProfile>, GetTenantGroupEmailConfirmationByTenantIdHandler>();
            services.AddQueryHandler<QueryTermsAndConditionsByTenantId, GetTermsAndConditionsByTenantIdDto, GetTermsAndConditionsByTenantIdQueryHandler>();
            services.AddQueryHandler<QueryTenantStudentDetailById, GetTenantStudentDetailByIdDto, GetTenantStudentDetailByIdQueryHandler>();

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
            
            services.AddCommandHandler<CreateTenantApplicationRequestCommand, CreateTenantApplicationRequestCommandHandler, CreateTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<UpdateTenantApplicationRequestCommand, UpdateTenantApplicationRequestCommandHandler, UpdateTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestCommand, DeleteTenantApplicationRequestCommandHandler, DeleteTenantApplicationRequestCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestGuarantorCommand, DeleteTenantApplicationRequestGuarantorCommandHandler, DeleteTenantApplicationRequestGuarantorCommandValidator>();
            services.AddCommandHandler<DeleteTenantApplicationRequestCoapplicantCommand, DeleteTenantApplicationRequestCoapplicantCommandHandler, DeleteTenantApplicationRequestCoapplicantCommandValidator>();
            services.AddCommandHandler<CreateGroupApplicationCommand, CreateGroupApplicationCommandHandler, CreateGroupApplicationCommandValidator>();
            services.AddCommandHandler<UpdateGroupApplicationCommand, UpdateGroupApplicationCommandHandler, UpdateGroupApplicationCommandValidator>();
            services.AddCommandHandler<CreateTenantApplicationTypeCommand, CreateTenantApplicationTypeCommandHandler, CreateTenantApplicationTypeCommandValidator>();
            services.AddCommandHandler<UpdateTenantApplicationTypeCommand, UpdateTenantApplicationTypeCommandHandler, UpdateTenantApplicationTypeCommandValidator>();
            services.AddCommandHandler<DeleteTenantCoSignerFromGroupApplicationProfileCommand, DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler, DeleteTenantCoSignerFromGroupApplicationProfileCommandValidator>();
            services.AddCommandHandler<DeleteTenantStudentDetailCommand, DeleteTenantStudentDetailCommandHandler, DeleteTenantStudentDetailCommandValidator>();

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

using System;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
	public class GetTenantProfileSummaryByIdQueryHandler : IQueryHandler<QueryTenantProfileSummaryById, GetTenantProfileSummaryDto>
    {
        private readonly IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> _getTenantApllicationDetailsByIdHandler;

        private readonly IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> _getTenantPersonalInfoByIdHandler;

        private readonly IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> _getTenantContactInformationByTenantId;

        private readonly IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> _getTenantEmployeeIncomesById;

        private readonly IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto> _getTenantRentalHistoriesByIdHandler;

        private readonly IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto> _getTenantFeedbackViaLandlordByIdQueryHandler;

        private readonly IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto> _getTenantAcquaintanceReferralByIdHandler;

        private readonly IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto> _getReferralInfoHandler;

        private readonly ILogger<GetTenantProfileSummaryByIdQueryHandler> _logger;

        public GetTenantProfileSummaryByIdQueryHandler(
            IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> getTenantApllicationDetailsByIdHandler,
            IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> getTenantPersonalInfoByIdHandler,
            IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> getTenantContactInformationByTenantId,
            IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> getTenantEmployeeIncomesById,
            IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto> getTenantRentalHistoriesByIdHandler,
            IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto> getTenantFeedbackViaLandlordByIdQueryHandler,
            IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto> getTenantAcquaintanceReferralByIdHandler,
             IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto> getReferralInfoHandler,
             ILogger<GetTenantProfileSummaryByIdQueryHandler> logger)
		{
            _getTenantApllicationDetailsByIdHandler = getTenantApllicationDetailsByIdHandler;

            _getTenantPersonalInfoByIdHandler = getTenantPersonalInfoByIdHandler;

            _getTenantContactInformationByTenantId = getTenantContactInformationByTenantId;

            _getTenantEmployeeIncomesById = getTenantEmployeeIncomesById;

            _getTenantRentalHistoriesByIdHandler = getTenantRentalHistoriesByIdHandler;

            _getTenantFeedbackViaLandlordByIdQueryHandler = getTenantFeedbackViaLandlordByIdQueryHandler;

            _getTenantAcquaintanceReferralByIdHandler = getTenantAcquaintanceReferralByIdHandler;

            _getReferralInfoHandler = getReferralInfoHandler;

            _logger = logger;
        }

        public async Task<GetTenantProfileSummaryDto> HandleAsync(QueryTenantProfileSummaryById query)
        {
            _logger.LogInformation("Call QueryTenantApplicationDetailsById with if {Id} ", query.Id);
            var GetTenantApplicationDetailsDto = await _getTenantApllicationDetailsByIdHandler.HandleAsync(new QueryTenantApplicationDetailsById(query.Id));
            _logger.LogInformation("Call QueryTenantBasicInformationById with if {Id} ", query.Id);
            var GetTenantBasicInformationDto = await _getTenantPersonalInfoByIdHandler.HandleAsync(new QueryTenantBasicInformationById(query.Id));
            _logger.LogInformation("Call QueryTenantContactInformationByTenantId with if {Id} ", query.Id);
            var GetTenantContactInformationDto = await _getTenantContactInformationByTenantId.HandleAsync(new QueryTenantContactInformationByTenantId(query.Id));
            _logger.LogInformation("Call QueryTenantEmployeeIncomesById with if {Id} ", query.Id);
            var GetTenantEmployeeIncomesDto = await _getTenantEmployeeIncomesById.HandleAsync(new QueryTenantEmployeeIncomesById(query.Id));
            _logger.LogInformation("Call QueryTenantRentalHistoriesById with if {Id} ", query.Id);
            var GetTenantRentalHistoriesDto = await _getTenantRentalHistoriesByIdHandler.HandleAsync(new QueryTenantRentalHistoriesById(query.Id));

            _logger.LogInformation("Call RentalHistories with if {Id} ", query.Id);
            List<GetTenantFeedbackViaLandlordDto> rentalHistoriesfeedback = new List<GetTenantFeedbackViaLandlordDto>();
            if (GetTenantRentalHistoriesDto?.RentalHistories != null)
            {
                foreach (var item in GetTenantRentalHistoriesDto.RentalHistories)
                {
                    var rentalHistoryfeedback = await _getTenantFeedbackViaLandlordByIdQueryHandler.HandleAsync(new QueryTenantFeedbackViaLandlordById(item.DocumentId));
                    rentalHistoriesfeedback.Add(rentalHistoryfeedback);
                }
            }

            _logger.LogInformation("Call AquaintanceReferrals with if {Id} ", query.Id);
            var GetTenantAquaintanceReferralDto = await _getTenantAcquaintanceReferralByIdHandler.HandleAsync(new QueryTenantAcquaintanceReferralById(query.Id));
            List<GetAcquaintanceReferralFormInfoDto> otherReferrals = new List<GetAcquaintanceReferralFormInfoDto>();
            if (GetTenantAquaintanceReferralDto?.AquaintanceReferrals != null)
            {
                foreach (var item in GetTenantAquaintanceReferralDto.AquaintanceReferrals)
                {
                    var otherReferral = await _getReferralInfoHandler.HandleAsync(new QueryAcquaintanceReferralFormInfoByReferralId(item.Referralid));
                    otherReferrals.Add(otherReferral);
                }
            }


            var result = new GetTenantProfileSummaryDto()
            {
                ApplicationDetails = GetTenantApplicationDetailsDto,
                BasicInformation = GetTenantBasicInformationDto,
                ContactInformation = GetTenantContactInformationDto,
                EmployeeIncomes = GetTenantEmployeeIncomesDto,
                RentalHistories = GetTenantRentalHistoriesDto,
                RentalHistoriesfeedback = rentalHistoriesfeedback,
                OtherReferrals = otherReferrals
            };
            _logger.LogInformation("End TenantProfileSummary with Id {Id} ", query.Id);
            return result;
        }

    }
}


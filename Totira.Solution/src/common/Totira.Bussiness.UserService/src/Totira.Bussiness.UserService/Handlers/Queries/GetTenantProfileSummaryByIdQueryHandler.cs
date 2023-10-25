using System;
using Microsoft.AspNetCore.Mvc;
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

        public GetTenantProfileSummaryByIdQueryHandler(
            IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> getTenantApllicationDetailsByIdHandler,
            IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> getTenantPersonalInfoByIdHandler,
            IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> getTenantContactInformationByTenantId,
            IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> getTenantEmployeeIncomesById,
            IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto> getTenantRentalHistoriesByIdHandler,
            IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto> getTenantFeedbackViaLandlordByIdQueryHandler,
            IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto> getTenantAcquaintanceReferralByIdHandler,
             IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto> getReferralInfoHandler)
		{
            _getTenantApllicationDetailsByIdHandler = getTenantApllicationDetailsByIdHandler;

            _getTenantPersonalInfoByIdHandler = getTenantPersonalInfoByIdHandler;

            _getTenantContactInformationByTenantId = getTenantContactInformationByTenantId;

            _getTenantEmployeeIncomesById = getTenantEmployeeIncomesById;

            _getTenantRentalHistoriesByIdHandler = getTenantRentalHistoriesByIdHandler;

            _getTenantFeedbackViaLandlordByIdQueryHandler = getTenantFeedbackViaLandlordByIdQueryHandler;

            _getTenantAcquaintanceReferralByIdHandler = getTenantAcquaintanceReferralByIdHandler;

            _getReferralInfoHandler = getReferralInfoHandler;
        }

        public async Task<GetTenantProfileSummaryDto> HandleAsync(QueryTenantProfileSummaryById query)
        {
            var GetTenantApplicationDetailsDto = await _getTenantApllicationDetailsByIdHandler.HandleAsync(new QueryTenantApplicationDetailsById(query.Id));
            var GetTenantBasicInformationDto = await _getTenantPersonalInfoByIdHandler.HandleAsync(new QueryTenantBasicInformationById(query.Id));
            var GetTenantContactInformationDto = await _getTenantContactInformationByTenantId.HandleAsync(new QueryTenantContactInformationByTenantId(query.Id));
            var GetTenantEmployeeIncomesDto = await _getTenantEmployeeIncomesById.HandleAsync(new QueryTenantEmployeeIncomesById(query.Id));
            var GetTenantRentalHistoriesDto = await _getTenantRentalHistoriesByIdHandler.HandleAsync(new QueryTenantRentalHistoriesById(query.Id));

            List<GetTenantFeedbackViaLandlordDto> rentalHistoriesfeedback = new List<GetTenantFeedbackViaLandlordDto>();
            if (GetTenantRentalHistoriesDto?.RentalHistories != null)
            {
                foreach (var item in GetTenantRentalHistoriesDto.RentalHistories)
                {
                    var rentalHistoryfeedback = await _getTenantFeedbackViaLandlordByIdQueryHandler.HandleAsync(new QueryTenantFeedbackViaLandlordById(item.DocumentId));
                    rentalHistoriesfeedback.Add(rentalHistoryfeedback);
                }
            }

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

            return result;
        }

    }
}


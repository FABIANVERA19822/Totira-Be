using System;
namespace Totira.Bussiness.UserService.DTO
{
	public class GetTenantProfileSummaryDto
	{
        public GetTenantApplicationDetailsDto? ApplicationDetails { get; set; }
        public GetTenantBasicInformationDto? BasicInformation { get; set; } 
        public GetTenantContactInformationDto? ContactInformation { get; set; }
        public GetTenantEmployeeIncomesDto? EmployeeIncomes { get; set; } 
        public GetTenantRentalHistoriesDto? RentalHistories { get; set; }
        public List<GetTenantFeedbackViaLandlordDto> RentalHistoriesfeedback { get; set; } = new List<GetTenantFeedbackViaLandlordDto>();
        public List<GetAcquaintanceReferralFormInfoDto> OtherReferrals { get; set; } = new List<GetAcquaintanceReferralFormInfoDto>();
    }

}


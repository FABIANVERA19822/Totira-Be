using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;
using Totira.Support.Otp.DTO;
using static Totira.Bussiness.UserService.Queries.QueryTenantGroupInventees;

namespace Totira.Services.UserService.Controllers
{
    public class UserController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> _getTenantApllicationDetailsByIdHandler;
        private readonly IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> _getTenantPersonalInfoByIdHandler;
        private readonly IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> _getTenantProfileImageByIdHandler;
        private readonly IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto> _getTenantAcquaintanceReferralByIdHandler;
        private readonly IQueryHandler<QueryTenantAcquaintanceReferralEmailsById, GetTenantAquaintanceReferralEmailsDto> _getTenantAcquaintanceReferralEmailsByIdHandler;
        private readonly IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto> _getTenantFeedbackViaLandlordByIdQueryHandler;
        private readonly IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto> _getTenantRentalHistoriesByIdHandler;
        private readonly IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> _getTenantEmployeeIncomesById;
        private readonly IQueryHandler<QueryTenantShareProfileByTenantId, GetTenantShareProfileDto> _getTenantShareProfileById;
        private readonly IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> _getTenantContactInformationByTenantId;
        private readonly IQueryHandler<QueryTenantEmploymentReferenceById, GetTenantEmploymentReferenceDto> _getTenantEmploymentReferenceByIdHandler;
        private readonly IQueryHandler<QueryTenantEmployeeIncomeById, GetTenantEmployeeIncomeDto> _getTenantEmployeeIncomeById;
        private readonly IQueryHandler<QueryTenantProfileSummaryById, GetTenantProfileSummaryDto> _getTenantProfileSummaryById;
        private readonly IQueryHandler<QueryTenantGroupApplicationSummaryById, GetTenantGroupApplicationSummaryDto> _getTenantGroupApplicationSummaryById;
        private readonly IQueryHandler<QueryTenantApplicationListPageById, GetTenantApplicationListPageDto> _getTenantApplicationListPageById;
        private readonly IQueryHandler<QueryTenantInformationForCertnApplicationById, GetTenantInformationForCertnApplicationDto> _getTenantInformationForCertnApplicationById;
        private readonly IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>> _getTenantProfileProgress;
        private readonly IQueryHandler<QueryTenantProfileFunnelByTenantId, Dictionary<string, bool>> _getTenantProfileFunnel;
        private readonly IQueryHandler<QueryTenantShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto> _checkTenantShareProfileForCheckCode;
        private readonly IQueryHandler<QueryTenantGroupShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto> _checkTenantGroupShareProfileForCheckCode;
        private readonly IQueryHandler<QueryValidateLinkOtp, ValidateLinkOtpDto> _validateLinkOtp;
        private readonly IQueryHandler<QueryValidateOtp, ValidateOtpDto> _validateOtp;
        private readonly IQueryHandler<QueryVerificationsProcessStatusById, string> _getTenantVerificationProcessStatusByIdHandler;
        private readonly IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> _getTenantVerifiedProfileById;
        private readonly ILogger<UserController> _logger;
        private readonly IQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto> _checkUserIsExistByEmail;
        private readonly IQueryHandler<QueryTenantGroupInventeesByTenantId, List<TenantGroupApplicationProfile>> _tenantGroupInventeesByTenantId;
        private readonly IQueryHandler<QueryTenantGroupInventees, ListTenantGroupApplicationProfile> _tenantGroupInventees;
        private readonly IQueryHandler<QueryApplicationTypeByTenantId, GetTenantApplicationTypeByDto> _getTenantApplicationTypeByIdHandler;
        private readonly IQueryHandler<QueryTenantGroupEmailConfirmationByTenantId, List<TenantGroupApplicationProfile>> _tenantGroupEmailConfirmation;
        private readonly IQueryHandler<QueryTermsAndConditionsByTenantId, GetTermsAndConditionsByTenantIdDto> _getTermsAndConditionsByTenantIdHandler;
        private readonly IQueryHandler<QueryTenantStudentDetailById, GetTenantStudentDetailByIdDto> _getTenantStudentDetailById;
        private readonly IQueryHandler<QueryCurrentJobStatusByTenantId, GetTenantCurrentJobStatusByDto> _getTenantCurrentJobByIdHandler;
        #region Landlord
        private readonly IQueryHandler<QueryLandlordBasicInformationById, GetLandlordBasicInformationDto> _getLandlordBasicInformationByIdHandler;
        private readonly IQueryHandler<QueryLandlordIdentityInformationById, GetLandlordIdentityInformationDto> _getLandlordIdentityInformationByIdHandler;
        private readonly IQueryHandler<QueryPropertyClaimsDisplayByLandlordId, GetLandlordClaimsDisplayDto> _getPropertyClaimsDisplayByLanlordIdHandler;
        private readonly IQueryHandler<QueryPendingClaimsByLandlordId, IEnumerable<GetPendingLandlordClaimsDto>> _getPendingLandlordClaimsByLanlordIdHandler;
        #endregion

        public UserController(
            ILogger<UserController> logger,
            IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> getTenantPersonalInfoByIdHandler,
            IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> getTenantProfileImageByIdHandler,
            IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> getTenantApllicationDetailsByIdHandler,
            IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto> getTenantAcquaintanceReferralByIdHandler,
            IQueryHandler<QueryTenantAcquaintanceReferralEmailsById, GetTenantAquaintanceReferralEmailsDto> getTenantAcquaintanceReferralEmailsByIdHandler,
            IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto> getTenantRentalHistoriesByIdHandler,
            IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto> getTenantFeedbackViaLandlordByIdQueryHandler,
            IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> getTenantEmployeeIncomesById,
            IQueryHandler<QueryTenantShareProfileByTenantId, GetTenantShareProfileDto> getTenantShareProfileById,
            IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> getTenantContactInformationByTenantId,
            IQueryHandler<QueryTenantEmploymentReferenceById, GetTenantEmploymentReferenceDto> getTenantEmploymentReferenceByIdHandler,
            IQueryHandler<QueryTenantEmployeeIncomeById, GetTenantEmployeeIncomeDto> getTenantEmployeeIncomeById,
            IQueryHandler<QueryTenantProfileSummaryById, GetTenantProfileSummaryDto> getTenantProfileSummaryById,

            IQueryHandler<QueryTenantGroupApplicationSummaryById, GetTenantGroupApplicationSummaryDto> getTenantGroupApplicationSummaryById,

            IQueryHandler<QueryTenantApplicationListPageById, GetTenantApplicationListPageDto> getTenantApplicationListPageById,


            IQueryHandler<QueryTenantInformationForCertnApplicationById, GetTenantInformationForCertnApplicationDto> getTenantInformationForCertnApplicationById,

            IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>> getTenantProfileProgress,
            IQueryHandler<QueryTenantProfileFunnelByTenantId, Dictionary<string, bool>> getTenantProfilefunnel,
            IQueryHandler<QueryTenantShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto> checkTenantShareProfileForCheckCode,
            IQueryHandler<QueryTenantGroupShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto> checkTenantGroupShareProfileForCheckCode,
            IQueryHandler<QueryVerificationsProcessStatusById, string> getTenantVerificationProcessStatusByIdHandler,
            IQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto> checkUserIsExistByEmail,
            IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> getTenantVerifiedProfileById,
            IQueryHandler<QueryTenantGroupInventeesByTenantId, List<TenantGroupApplicationProfile>> tenantGroupInventeesByTenantId,
            IQueryHandler<QueryTenantGroupInventees, ListTenantGroupApplicationProfile> tenantGroupInventees,
            IQueryHandler<QueryApplicationTypeByTenantId, GetTenantApplicationTypeByDto> getTenantApplicationTypeByIdHandler,
            IQueryHandler<QueryTenantGroupEmailConfirmationByTenantId, List<TenantGroupApplicationProfile>> tenantGroupEmailConfirmation,
            IQueryHandler<QueryTermsAndConditionsByTenantId, GetTermsAndConditionsByTenantIdDto> getTermsAndConditionsByTenantIdHandler,
            IQueryHandler<QueryTenantStudentDetailById, GetTenantStudentDetailByIdDto> getTenantStudentDetailById,
            IQueryHandler<QueryCurrentJobStatusByTenantId, GetTenantCurrentJobStatusByDto> getTenantCurrentJobByIdHandler,
            IQueryHandler<QueryValidateOtp, ValidateOtpDto> validateOtp, 
            IQueryHandler<QueryValidateLinkOtp, ValidateLinkOtpDto> validateLinkOtp,
           /// Landlord handlers
            IQueryHandler<QueryLandlordBasicInformationById, GetLandlordBasicInformationDto> getLandlordBasicInformationByIdHandler,
            IQueryHandler<QueryLandlordIdentityInformationById, GetLandlordIdentityInformationDto> getLandlordIdentityInformationByIdHandler,
            IQueryHandler<QueryPropertyClaimsDisplayByLandlordId, GetLandlordClaimsDisplayDto> getPropertyClaimsDisplayByLanlordIdHandler,           
            IQueryHandler<QueryPendingClaimsByLandlordId, IEnumerable<GetPendingLandlordClaimsDto>> getPendingLandlordClaimsByLanlordIdHandler)            
        {
            _getTenantPersonalInfoByIdHandler = getTenantPersonalInfoByIdHandler;
            _getTenantProfileImageByIdHandler = getTenantProfileImageByIdHandler;
            _getTenantApllicationDetailsByIdHandler = getTenantApllicationDetailsByIdHandler;
            _getTenantAcquaintanceReferralByIdHandler = getTenantAcquaintanceReferralByIdHandler;
            _getTenantAcquaintanceReferralEmailsByIdHandler = getTenantAcquaintanceReferralEmailsByIdHandler;
            _getTenantFeedbackViaLandlordByIdQueryHandler = getTenantFeedbackViaLandlordByIdQueryHandler;
            _getTenantRentalHistoriesByIdHandler = getTenantRentalHistoriesByIdHandler;
            _getTenantEmployeeIncomesById = getTenantEmployeeIncomesById;
            _getTenantShareProfileById = getTenantShareProfileById;
            _getTenantEmploymentReferenceByIdHandler = getTenantEmploymentReferenceByIdHandler;
            _logger = logger;
            _getTenantContactInformationByTenantId = getTenantContactInformationByTenantId;
            _getTenantEmployeeIncomeById = getTenantEmployeeIncomeById;
            _getTenantProfileSummaryById = getTenantProfileSummaryById;
            _getTenantGroupApplicationSummaryById = getTenantGroupApplicationSummaryById;
            _getTenantApplicationListPageById = getTenantApplicationListPageById;
            _getTenantInformationForCertnApplicationById = getTenantInformationForCertnApplicationById;
            _getTenantProfileProgress = getTenantProfileProgress;
            _getTenantProfileFunnel = getTenantProfilefunnel;
            _checkTenantShareProfileForCheckCode = checkTenantShareProfileForCheckCode;
            _checkTenantGroupShareProfileForCheckCode = checkTenantGroupShareProfileForCheckCode;
            _getTenantVerificationProcessStatusByIdHandler = getTenantVerificationProcessStatusByIdHandler;

            _checkUserIsExistByEmail = checkUserIsExistByEmail;
            _getTenantVerifiedProfileById = getTenantVerifiedProfileById;
            _tenantGroupInventeesByTenantId = tenantGroupInventeesByTenantId;
            _tenantGroupInventees = tenantGroupInventees;
            _getTenantApplicationTypeByIdHandler = getTenantApplicationTypeByIdHandler;
            _tenantGroupEmailConfirmation = tenantGroupEmailConfirmation;
            _getTermsAndConditionsByTenantIdHandler = getTermsAndConditionsByTenantIdHandler;
            _getTenantStudentDetailById = getTenantStudentDetailById;
            _getTenantCurrentJobByIdHandler = getTenantCurrentJobByIdHandler;
            _validateOtp = validateOtp;
            _validateLinkOtp = validateLinkOtp;

            //Landlord
            _getLandlordBasicInformationByIdHandler = getLandlordBasicInformationByIdHandler;
            _getLandlordIdentityInformationByIdHandler = getLandlordIdentityInformationByIdHandler;
            _getPropertyClaimsDisplayByLanlordIdHandler = getPropertyClaimsDisplayByLanlordIdHandler;
            _getPendingLandlordClaimsByLanlordIdHandler = getPendingLandlordClaimsByLanlordIdHandler;
        }

        #region Basic Information

        [HttpGet("tenant/{id}/basic-info")]
        [ProducesResponseType(typeof(GetTenantBasicInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantBasicInformationDto>> GetBasicInformation(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantPersonalInfoByIdHandler.HandleAsync(new QueryTenantBasicInformationById(id));
        }

        #endregion Basic Information

        [HttpGet("tenant/AcquaintanceReferral/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantAquaintanceReferralDto>> GetTenantaAquaintanceReferral(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantAcquaintanceReferralByIdHandler.HandleAsync(new QueryTenantAcquaintanceReferralById(id));
        }

        [HttpGet("tenant/ProfileSummary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantProfileSummaryDto>> GetTenantProfileSummary(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantProfileSummaryById.HandleAsync(new QueryTenantProfileSummaryById(id));
        }

        [HttpGet("tenant/GroupApplicationSummary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantGroupApplicationSummaryDto>> GetTenantGroupApplicationSummary(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantGroupApplicationSummaryById.HandleAsync(new QueryTenantGroupApplicationSummaryById(id));
        }

        [HttpGet("tenant/ApplicationListPage/{id}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationListPageDto>> GetTenantApplicationListPage(Guid id, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantApplicationListPageById.HandleAsync(new QueryTenantApplicationListPageById(id, pageNumber, pageSize));
        }

        [HttpGet("tenant/AcquaintanceReferralEmails/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantAquaintanceReferralEmailsDto>> GetTenantaAquaintanceReferralEmails(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantAcquaintanceReferralEmailsByIdHandler.HandleAsync(new QueryTenantAcquaintanceReferralEmailsById(id));
        }

        [HttpGet("tenant/RentalHistories/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantRentalHistoriesDto>> GetTenantRentalHistories(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantRentalHistoriesByIdHandler.HandleAsync(new QueryTenantRentalHistoriesById(id));
        }

        [HttpGet("tenant/EmploymentReference/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantEmploymentReferenceDto>> GetTenantEmploymentReference(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantEmploymentReferenceByIdHandler.HandleAsync(new QueryTenantEmploymentReferenceById(id));
        }

        [HttpGet("tenant/{id}/ApplicationDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationDetailsDto>> GetTenantApplicationDetails(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantApllicationDetailsByIdHandler.HandleAsync(new QueryTenantApplicationDetailsById(id));
        }

        [HttpGet("tenant/TenantFeedbackViaLandlord/{landlordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantFeedbackViaLandlordDto>> GetTenantFeedbackViaLandlord(Guid landlordId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return await _getTenantFeedbackViaLandlordByIdQueryHandler.HandleAsync(new QueryTenantFeedbackViaLandlordById(landlordId));
        }

        [HttpGet("tenant/{id}/incomes")]
        public async Task<ActionResult<GetTenantEmployeeIncomesDto>> GetTenantIncomes(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantEmployeeIncomesById.HandleAsync(new QueryTenantEmployeeIncomesById(id));
        }

        [HttpGet("tenant/{id}/{encryptedAccessCode}/{email}/shareProfile")]
        public async Task<ActionResult<GetTenantShareProfileDto>> GetTenantShareProfile(Guid id, string encryptedAccessCode, string email)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantShareProfileById.HandleAsync(new QueryTenantShareProfileByTenantId(id, encryptedAccessCode, email));
        }

        [HttpGet("tenant/{tenantId}/employee-incomes/{incomeId}")]
        public async Task<ActionResult<GetTenantEmployeeIncomeDto>> GetTenantEmployeeIncome(Guid tenantId, Guid incomeId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantEmployeeIncomeById.HandleAsync(new QueryTenantEmployeeIncomeById(tenantId, incomeId));
        }

        [HttpGet("tenant/profileimage/{id}")]
        public async Task<ActionResult<GetTenantProfileImageDto>> GetTenantProfileImage(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantProfileImageByIdHandler.HandleAsync(new QueryTenantProfileImageById(id));
        }

        [HttpGet("tenant/contactinfo/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantContactInformationDto>> GetTenantContactInformation(Guid tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantContactInformationByTenantId.HandleAsync(new QueryTenantContactInformationByTenantId(tenantId));
        }

        [HttpGet]
        [Route("tenant/{tenantId}/certn-request-info")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantInformationForCertnApplicationDto>> GetTenantSummary(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var basicInformation = GetBasicInformation(tenantId);
            if (basicInformation.Result.Value.FirstName == null)
            {
                _logger.LogWarning($"Basic information is missing for Tenant {tenantId}", tenantId);
                return Conflict($"Basic information is missing for Tenant {tenantId}");
            }
            if (basicInformation.Result.Value.SocialInsuranceNumber != null && basicInformation.Result.Value.SocialInsuranceNumber.Length != 9)
            {
                _logger.LogWarning($"Wrong Social Insurance Number, must be 9 characters or null for Tenant {tenantId}", tenantId);
                return Conflict($"Wrong Social Insurance Number, must be 9 characters or null {tenantId}");
            }

            var contactInformation = GetTenantContactInformation(tenantId);
            if (contactInformation.Result.Value.StreetAddress == null || contactInformation.Result.Value.StreetAddress == "")
            {
                _logger.LogWarning($"Contact Information is missing for Tenant {tenantId}", tenantId);
                return Conflict($"Contact Information is missing for Tenant {tenantId}");
            }

            return await _getTenantInformationForCertnApplicationById.HandleAsync(new QueryTenantInformationForCertnApplicationById(tenantId));
        }

        [HttpGet("tenant/profileprogress/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, int>>> GetTenantProfileProgress(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantProfileProgress.HandleAsync(new QueryTenantProfileProgressByTenantId(id));
        }

        [HttpGet("tenant/profilefunnel/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, bool>>> GetTenantProfileFunnel(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantProfileFunnel.HandleAsync(new QueryTenantProfileFunnelByTenantId(id));
        }

        [HttpGet("tenant/verifiedprofile/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantVerifiedbyProfileDto>> GetTenantVerifiedProfile(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var incomes = GetTenantIncomes(id);

            return await _getTenantVerifiedProfileById.HandleAsync(new QueryTenantVerifiedProfileById(id, incomes.Result.Value));
        }

        [HttpGet("ValidateLinkOtp/{otpId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ValidateLinkOtpDto>> ValidateLinkOtp(Guid otpId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _validateLinkOtp.HandleAsync(new QueryValidateLinkOtp(otpId));
        }

        [HttpGet("ValidateOtp/{otpId}/{accessCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ValidateOtpDto>> ValidateOtp(Guid otpId, string accessCode)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _validateOtp.HandleAsync(new QueryValidateOtp(otpId, accessCode));
        }

        [HttpGet("CheckTenantShareProfileForCheckCode/{tenantId}/{accessCode}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantShareProfileForCheckCodeAndEmailDto>> CheckTenantShareProfileForCheckCode(Guid tenantId, int accessCode, string email)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _checkTenantShareProfileForCheckCode.HandleAsync(new QueryTenantShareProfileForCheckCodeAndEmail(tenantId, accessCode, email));
        }

        [HttpGet("GetVerificationProcessStatus/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetVerificationProcessStatus(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantVerificationProcessStatusByIdHandler.HandleAsync(new QueryVerificationsProcessStatusById(tenantId));
        }

        [HttpGet("CheckUserIsExistByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserIsExistByEmailDto>> CheckUserIsExistByEmail(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _checkUserIsExistByEmail.HandleAsync(new QueryCheckUserIsExistByEmail(email));
        }

        [HttpGet("GetTenantGroupInventeesByTenantId/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<TenantGroupApplicationProfile>>> GetTenantGroupInventeesByTenantId(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _tenantGroupInventeesByTenantId.HandleAsync(new QueryTenantGroupInventeesByTenantId(tenantId));
        }

        [HttpGet("GetTenantGroupEmailConfirmation/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<TenantGroupApplicationProfile>>> GetTenantGroupEmailConfirmationByTenantId(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _tenantGroupEmailConfirmation.HandleAsync(new QueryTenantGroupEmailConfirmationByTenantId(tenantId));
        }

        [HttpGet("GetTenantApplicationTypeByTenantId/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationTypeByDto>> GetTenantApplicationTypeByTenantId(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantApplicationTypeByIdHandler.HandleAsync(new QueryApplicationTypeByTenantId(tenantId));
        }

        [HttpGet("GetTenantGroupInventees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListTenantGroupApplicationProfile>> GetAllTenantGroupInvitees()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            return await _tenantGroupInventees.HandleAsync(new QueryTenantGroupInventees(EnumTenantGroupSortBy.CreatedOn, 1, 20));
        }

        [HttpGet("tenant/{tenantId}/terms-and-conditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTermsAndConditionsByTenantIdDto>> GetTermsAndConditions(Guid tenantId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }

            return await _getTermsAndConditionsByTenantIdHandler.HandleAsync(new QueryTermsAndConditionsByTenantId(tenantId));
        }

        [HttpGet("tenant/{tenantId}/study-details/{studyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantStudentDetailByIdDto>> GetTenantStudentDetailById(Guid tenantId, Guid studyId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }

            return await _getTenantStudentDetailById.HandleAsync(new QueryTenantStudentDetailById(tenantId, studyId));
        }

        [HttpGet("GetTenantCurrentJobStatusByTenantId/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantCurrentJobStatusByDto>> GetTenantCurrentJobStatusByTenantId(Guid tenantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantCurrentJobByIdHandler.HandleAsync(new QueryCurrentJobStatusByTenantId(tenantId));
        }

        [HttpGet("CheckTenantGroupShareProfileForCheckCode/{tenantId}/{accessCode}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantShareProfileForCheckCodeAndEmailDto>> CheckTenantGroupShareProfileForCheckCode(Guid tenantId, int accessCode, string email)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _checkTenantGroupShareProfileForCheckCode.HandleAsync(new QueryTenantGroupShareProfileForCheckCodeAndEmail(tenantId, accessCode, email));
        }

        #region Landlord Endpoints
        [HttpGet("landlord/{id}/basic-info")]
        [ProducesResponseType(typeof(GetLandlordBasicInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLandlordBasicInformationDto>> GetLandlordBasicInformation(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getLandlordBasicInformationByIdHandler.HandleAsync(new QueryLandlordBasicInformationById(id));
        }

        [HttpGet("landlord/{id}/identity-info")]
        [ProducesResponseType(typeof(GetLandlordIdentityInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLandlordIdentityInformationDto>> GetLandlordIdentity(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getLandlordIdentityInformationByIdHandler.HandleAsync(new QueryLandlordIdentityInformationById(id));
        }

        [HttpGet("landlord/{landlordId}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLandlordClaimsDisplayDto>> GetLandlordClaimedProperties(Guid landlordId,int pageNumber, int pageSize)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _getPropertyClaimsDisplayByLanlordIdHandler.HandleAsync(new QueryPropertyClaimsDisplayByLandlordId(landlordId, pageNumber, pageSize)));
        }
        
        [HttpGet("landlord/{landlordId}/pendingClaimsFromLandlord")]
        [ProducesResponseType(typeof(IEnumerable<GetPendingLandlordClaimsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetPendingLandlordClaimsDto>>> GetPendingClaimsFromLandlord(Guid landlordId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(await _getPendingLandlordClaimsByLanlordIdHandler.HandleAsync(new QueryPendingClaimsByLandlordId(landlordId)));
        }
        #endregion
    }
}
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetAcquaintanceReferralFormInfoByReferralIdQueryHandler : IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto>
    {
        private readonly IOptions<FrontendSettings> _configuration;
        private readonly ILogger<CreateTenantAcquaintanceReferralCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferralFormInfo, Guid> _tenantAcquaintanceReferralFormRepository;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly ICommonFunctions _commonFunctions;


        public GetAcquaintanceReferralFormInfoByReferralIdQueryHandler(
            IOptions<FrontendSettings> configuration,
            ILogger<CreateTenantAcquaintanceReferralCommandHandler> logger,
            IRepository<TenantAcquaintanceReferralFormInfo, Guid> tenantAcquaintanceReferralFormRepository,
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            ICommonFunctions commonFunctions)
        {
            _configuration = configuration;
            _logger = logger;
            _tenantAcquaintanceReferralFormRepository = tenantAcquaintanceReferralFormRepository;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _commonFunctions = commonFunctions;
        }
        public async Task<GetAcquaintanceReferralFormInfoDto> HandleAsync(QueryAcquaintanceReferralFormInfoByReferralId query)
        {
            var referralInfo = await _tenantAcquaintanceReferralsRepository.GetReferralById(query.Id);
            if(referralInfo is null)
                return null;

            var referralFormInfo = (await _tenantAcquaintanceReferralFormRepository.Get(r => r.ReferralId == query.Id)).FirstOrDefault();
            GetAcquaintanceReferralFormInfoDto response = new GetAcquaintanceReferralFormInfoDto
            {
                ReferralId = referralInfo.Id,
                ReferralName = referralInfo.FullName,
                TenantName = referralInfo.TenantName,
                Relationship = referralInfo.Relationship,
                OtherRelationship = referralInfo.OtherRelationship
            };
            var photo = await _commonFunctions.GetProfilePhoto(new QueryTenantProfileImageById(referralInfo.TenantId));
            response.PhotoLink = photo.Filename.FileUrl;

            if (referralFormInfo == null)
            {
                response.Status = referralInfo.CreatedOn.AddDays(_configuration.Value.timeoutExpirationDaysRequest) < DateTimeOffset.Now ? "Expired" : "Pending";
            }
            else
            {
                response.Status = "Complete";
                response.Comment = referralFormInfo.Comment;
                response.StarScore = referralFormInfo.Score;
            }

            return response;
        }
    }

    public static class ReferralById
    {
        public static async Task<TenantAcquaintanceReferral> GetReferralById(this IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepositorys, Guid referalId)
        {
            var data = (await tenantAcquaintanceReferralsRepositorys.Get(r => r.Referrals.Any(rf => rf.Id == referalId))).FirstOrDefault();
            return data is null ? null : data.Referrals.First(r => r.Id == referalId);
        }
    }
}

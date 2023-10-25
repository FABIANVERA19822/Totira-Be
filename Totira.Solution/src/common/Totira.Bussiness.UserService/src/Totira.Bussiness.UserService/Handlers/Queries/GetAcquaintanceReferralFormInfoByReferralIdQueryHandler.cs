using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
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
            GetAcquaintanceReferralFormInfoDto response = new GetAcquaintanceReferralFormInfoDto();
            var referralInfo = await _tenantAcquaintanceReferralsRepository.GetReferralById(query.Id);
            var referralFormInfo = (await _tenantAcquaintanceReferralFormRepository.Get((r => r.ReferralId == query.Id))).FirstOrDefault();
            response.ReferralId = referralInfo.Id;
            response.ReferralName = referralInfo.FullName;
            response.TenantName = referralInfo.TenantName;
            response.Relationship = referralInfo.Relationship;
            response.OtherRelationship = referralInfo.OtherRelationship;
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

            Expression<Func<TenantAcquaintanceReferrals, bool>> func = (r => r.Referrals.Any(rf => rf.Id == referalId));
            var data = (await tenantAcquaintanceReferralsRepositorys.Get(func)).FirstOrDefault();
            var referall = data.Referrals.First(r => r.Id == referalId);
            return referall;
        }



    }
}

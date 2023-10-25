using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantAcquaintanceReferralByIdQueryHandler : IQueryHandler<QueryTenantAcquaintanceReferralById, GetTenantAquaintanceReferralDto>
    {
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        public GetTenantAcquaintanceReferralByIdQueryHandler(IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository)
        {
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
        }

        public async Task<GetTenantAquaintanceReferralDto> HandleAsync(QueryTenantAcquaintanceReferralById query)
        {
            var info = await _tenantAcquaintanceReferralsRepository.GetByIdAsync(query.Id);

            if (info == null)
                return new GetTenantAquaintanceReferralDto { TenantId = query.Id };

            var referrals = new List<AquaintanceReferralDto>();

            if (info.Referrals != null && info.Referrals.Any())
                info.Referrals.ForEach(r => {
                    if (r.CreatedOn.AddMinutes(5) < DateTime.Now && r.Status == "Created") {
                        r.Status = "Expired";                        
                    }
                    
                    var referral = new AquaintanceReferralDto(
                        r.Id,
                        r.FullName,
                        r.Email,
                        r.Relationship == "Other"
                            ? r.OtherRelationship
                            : r.Relationship,
                        $"{r.Phone.CountryCode} {r.Phone.Number}",
                        r.Status);
                    referrals.Add(referral);
                    }
                );

            var result = new GetTenantAquaintanceReferralDto()
            {

                TenantId = info.Id,
                AquaintanceReferrals = referrals,
            };
            return result;


        }
    }
}


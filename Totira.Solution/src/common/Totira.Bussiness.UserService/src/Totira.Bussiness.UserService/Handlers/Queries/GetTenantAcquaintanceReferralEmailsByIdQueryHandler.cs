using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantAcquaintanceReferralEmailsByIdQueryHandler : IQueryHandler<QueryTenantAcquaintanceReferralEmailsById, GetTenantAquaintanceReferralEmailsDto>
    {
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepositorys;
        public GetTenantAcquaintanceReferralEmailsByIdQueryHandler(IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository)
        {
            _tenantAcquaintanceReferralsRepositorys = tenantAcquaintanceReferralsRepository;
        }

        public async Task<GetTenantAquaintanceReferralEmailsDto> HandleAsync(QueryTenantAcquaintanceReferralEmailsById query)
        {

            var emails = new List<AquaintanceReferralEmailDto>();
            emails = await _tenantAcquaintanceReferralsRepositorys.GetTenantAcquaintanceReferralEmailsByTenantId(query.Id);
            var result = new GetTenantAquaintanceReferralEmailsDto
            {
                TenantId = query.Id,
                AquaintanceReferralEmails = emails,
            };
            return result;
        }
    }
}
public static class ReferralEmails
{
    public static async Task<List<AquaintanceReferralEmailDto>> GetTenantAcquaintanceReferralEmailsByTenantId(this IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepositorys, Guid id)
    {

        var data = (await tenantAcquaintanceReferralsRepositorys.Get(u => u.Id == id)).FirstOrDefault();


        var emails = new List<AquaintanceReferralEmailDto>();

        if (data != null && data.Referrals!.Any())
            data.Referrals!.ForEach(r => emails.Add(new AquaintanceReferralEmailDto(r.Email)));


        return emails;
    }

}


using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class CheckUserIsExistByEmailHandler : IQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto>
{
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicRepository;
    private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
    private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;

    public CheckUserIsExistByEmailHandler(IRepository<TenantBasicInformation, Guid> tenantBasicRepository, IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository, IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository)
    {
        _tenantBasicRepository = tenantBasicRepository;
        _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
        _tenantGroupApplicationProfileRepository = tenantGroupApplicationProfileRepository;
    }

    public async Task<GetUserIsExistByEmailDto> HandleAsync(QueryCheckUserIsExistByEmail query)
    {
        var userIsExist = new GetUserIsExistByEmailDto();

        var tenant = (await _tenantBasicRepository.Get(x => x.TenantEmail == query.Email)).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

        var tenantApplicationRequest = (await _tenantApplicationRequestRepository.Get(item => item.Guarantor != null && item.Guarantor.Email.ToLower() == query.Email.ToLower())).FirstOrDefault();
        var tenantGroupApplicationProfile = (await _tenantGroupApplicationProfileRepository.Get(item => item.Email.ToLower() == query.Email.ToLower())).FirstOrDefault();
        var coapplicantsList = await _tenantApplicationRequestRepository.Get(item => item.Coapplicants != null && item.Coapplicants.Any());
        var tenantGroupApplicationProfileCoSigner = coapplicantsList.FirstOrDefault(item => item.Coapplicants != null && item.Coapplicants.Any() && item.Coapplicants.Select(s => s.Email.ToLower()).Contains(query.Email.ToLower()));

        if (tenantApplicationRequest is not null || tenantGroupApplicationProfile is not null || tenantGroupApplicationProfileCoSigner is not null)
        {
            userIsExist.IsExistApplicationProfileUser = true;
        }

        if (tenant is not null)
        {
            userIsExist.IsExistUser = true;
        }

        return userIsExist;
    }
}
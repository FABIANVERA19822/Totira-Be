using System.Linq.Expressions;
using System.Text;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;
using ApplicationDetailOccupants = Totira.Bussiness.UserService.DTO.ApplicationDetailOccupants;
namespace Totira.Bussiness.UserService.Handlers.Queries;

public class CheckUserIsExistByEmailHandler : IQueryHandler<QueryCheckUserIsExistByEmail, GetUserIsExistByEmailDto>
{
    private readonly IRepository<TenantContactInformation, Guid> _tenantContactRepository;
    public CheckUserIsExistByEmailHandler(IRepository<TenantContactInformation, Guid> tenantContactRepository)
    {
        _tenantContactRepository = tenantContactRepository;
    }

    public async Task<GetUserIsExistByEmailDto> HandleAsync(QueryCheckUserIsExistByEmail query)
    {

        var tenant = (await _tenantContactRepository.Get(item => item.Email.ToLower() == query.Email.ToLower())).FirstOrDefault();
        return new GetUserIsExistByEmailDto(tenant is not null);
    }
}
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class CheckTenantShareProfileCodeAndEmailHandler : IQueryHandler<QueryTenantShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto>
{
    private readonly IRepository<TenantShareProfile, Guid> _tenantShareProfileRepository;
    private readonly IEncryptionHandler _encryptionHandler;

    public CheckTenantShareProfileCodeAndEmailHandler(IRepository<TenantShareProfile, Guid> tenantShareProfileRepository, IEncryptionHandler encryptionHandler)
    {
        _tenantShareProfileRepository = tenantShareProfileRepository;
        _encryptionHandler = encryptionHandler;
    }

    public async Task<GetTenantShareProfileForCheckCodeAndEmailDto> HandleAsync(QueryTenantShareProfileForCheckCodeAndEmail query)
    {
        string decryptedAccessCode = _encryptionHandler.EncryptString(query.AccessCode.ToString());
        var tenantShareProfile = (await _tenantShareProfileRepository.Get(s => s.TenantId == query.TenantId
                                        && s.Email == query.Email && s.EncryptedAccessCode == decryptedAccessCode)).FirstOrDefault();


        if (tenantShareProfile is not null) {

            if (tenantShareProfile?.IsAcceptTermsAndConditions == true)  return new GetTenantShareProfileForCheckCodeAndEmailDto(false, "This code and email are entered before.", tenantShareProfile.TypeOfContact);
           
            else return new GetTenantShareProfileForCheckCodeAndEmailDto(true, tenantShareProfile.Id, tenantShareProfile.TypeOfContact);
            
        }
        else return new GetTenantShareProfileForCheckCodeAndEmailDto(false, "Invalid credentials. Please try again", string.Empty);
    }
}
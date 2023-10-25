using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Bussiness.UserService.DTO;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class CheckTenantGroupShareProfileCodeAndEmailHandler : IQueryHandler<QueryTenantGroupShareProfileForCheckCodeAndEmail, GetTenantShareProfileForCheckCodeAndEmailDto>
    {
        private readonly IRepository<TenantGroupApplicationShareProfile, Guid> _tenantGroupShareProfileRepository;
        private readonly IEncryptionHandler _encryptionHandler;

        public CheckTenantGroupShareProfileCodeAndEmailHandler(IRepository<TenantGroupApplicationShareProfile, Guid> tenantGroupShareProfileRepository, IEncryptionHandler encryptionHandler)
        {
            _tenantGroupShareProfileRepository = tenantGroupShareProfileRepository;
            _encryptionHandler = encryptionHandler;
        }

        public async Task<GetTenantShareProfileForCheckCodeAndEmailDto> HandleAsync(QueryTenantGroupShareProfileForCheckCodeAndEmail query)
        {
            string decryptedAccessCode = _encryptionHandler.EncryptString(query.AccessCode.ToString());
            var tenantShareProfile = (await _tenantGroupShareProfileRepository.Get(s => s.TenantId == query.TenantId
                                            && s.Email == query.Email && s.EncryptedAccessCode == decryptedAccessCode)).FirstOrDefault();


            if (tenantShareProfile is not null)
            {

                if (tenantShareProfile?.ContactAcceptedTermsAndConditions == true) return new GetTenantShareProfileForCheckCodeAndEmailDto(false, "This code and email are entered before.", tenantShareProfile.TypeOfContact);

                else return new GetTenantShareProfileForCheckCodeAndEmailDto(true, tenantShareProfile.Id, tenantShareProfile.TypeOfContact);

            }
            else return new GetTenantShareProfileForCheckCodeAndEmailDto(false, "Invalid credentials. Please try again", string.Empty);
        }
    }
}

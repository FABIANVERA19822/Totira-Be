using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantEmploymentReferenceByIdQueryHandler : IQueryHandler<QueryTenantEmploymentReferenceById, GetTenantEmploymentReferenceDto>
    {
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        public GetTenantEmploymentReferenceByIdQueryHandler(IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository)
        {
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
        }

        public async Task<GetTenantEmploymentReferenceDto> HandleAsync(QueryTenantEmploymentReferenceById query)
        {
            var info = await _tenantEmploymentReferenceRepository.GetByIdAsync(query.Id);

            var result =
                info != null ?
               new GetTenantEmploymentReferenceDto(info.Id, info.FirstName, info.LastName, info.JobTitle, info.Relationship, info.Email, new DTO.EmploymentReferencePhoneNumber(info.PhoneNumber.Number, info.PhoneNumber.CountryCode)) :
               new GetTenantEmploymentReferenceDto(query.Id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new DTO.EmploymentReferencePhoneNumber(string.Empty, string.Empty));

            return result;
        }
    }
}


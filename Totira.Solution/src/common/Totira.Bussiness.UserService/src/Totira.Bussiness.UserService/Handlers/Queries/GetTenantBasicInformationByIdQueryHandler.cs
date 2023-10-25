using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantBasicInformationByIdQueryHandler : IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto>
    {
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;

        public GetTenantBasicInformationByIdQueryHandler(IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository)
        {
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
        }

        public async Task<GetTenantBasicInformationDto> HandleAsync(QueryTenantBasicInformationById query)
        {
            var info = await _tenantBasicInformationRepository.GetByIdAsync(query.TenantId);

            if (info is null)
                return GetTenantBasicInformationDto.Empty(query.TenantId);

            var result = GetTenantBasicInformationDto.AdaptFrom(info);

            return result;
        }
    }
}

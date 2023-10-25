using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries.Landlord
{
    public class GetLandlordIdentityInformationByIdQueryHandler : IQueryHandler<QueryLandlordIdentityInformationById, GetLandlordIdentityInformationDto>
    {
        private readonly IRepository<LandlordIdentityInformation, Guid> _landlordIdentityInformationRepository;

        public GetLandlordIdentityInformationByIdQueryHandler(IRepository<LandlordIdentityInformation, Guid> landlordIdentityInformationRepository)
        {
            _landlordIdentityInformationRepository = landlordIdentityInformationRepository;
        }

        public async Task<GetLandlordIdentityInformationDto> HandleAsync(QueryLandlordIdentityInformationById query)
        {
            var info = await _landlordIdentityInformationRepository.GetByIdAsync(query.LandlordId);

            if (info is null)
                return GetLandlordIdentityInformationDto.Empty(query.LandlordId);

            var result = GetLandlordIdentityInformationDto.AdaptFrom(info);

            return result;
        }
    }
}

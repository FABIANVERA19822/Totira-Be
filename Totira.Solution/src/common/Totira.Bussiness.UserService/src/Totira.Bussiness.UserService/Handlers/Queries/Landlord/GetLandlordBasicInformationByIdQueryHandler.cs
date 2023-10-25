using static Totira.Support.Persistance.IRepository;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Queries.Landlord;

namespace Totira.Bussiness.UserService.Handlers.Queries.Landlord
{
    public class GetLandlordBasicInformationByIdQueryHandler : IQueryHandler<QueryLandlordBasicInformationById, GetLandlordBasicInformationDto>
    {
        private readonly IRepository<LandlordBasicInformation, Guid> _landlordBasicInformationRepository;

        public GetLandlordBasicInformationByIdQueryHandler(IRepository<LandlordBasicInformation, Guid> landlordBasicInformationRepository)
        {
            _landlordBasicInformationRepository = landlordBasicInformationRepository;
        }

        public async Task<GetLandlordBasicInformationDto> HandleAsync(QueryLandlordBasicInformationById query)
        {
            var info = await _landlordBasicInformationRepository.GetByIdAsync(query.LandlordId);

            if (info is null)
                return GetLandlordBasicInformationDto.Empty(query.LandlordId);

            var result = GetLandlordBasicInformationDto.AdaptFrom(info);

            return result;
        }
    }
}

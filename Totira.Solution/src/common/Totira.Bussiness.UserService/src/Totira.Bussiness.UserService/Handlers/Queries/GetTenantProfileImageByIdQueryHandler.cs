using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantProfileImageByIdQueryHandler : IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto>
    {

        private readonly ICommonFunctions _commonFunctions;
        public GetTenantProfileImageByIdQueryHandler(ICommonFunctions commonHandler)
        {
            _commonFunctions = commonHandler;
        }
        public async Task<GetTenantProfileImageDto> HandleAsync(QueryTenantProfileImageById query)
        {
            return await _commonFunctions.GetProfilePhoto(query);

        }


    }
}
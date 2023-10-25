using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.DTO.Landlord;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Bussiness.UserService.DTO.Common;
using Microsoft.Extensions.Logging;

namespace Totira.Bussiness.UserService.Handlers.Queries.Landlord
{
    public class GetPendingClaimsByLandlordIdQueryHandler : IQueryHandler<QueryPendingClaimsByLandlordId, IEnumerable<GetPendingLandlordClaimsDto>>
    {
        private readonly IRepository<LandlordPropertyClaim, Guid> _landlordClaimRepository;
        private readonly ILogger<GetPendingClaimsByLandlordIdQueryHandler> _logger;

        public GetPendingClaimsByLandlordIdQueryHandler(IRepository<LandlordPropertyClaim, Guid> landlordclaimRepository,
                                                        ILogger<GetPendingClaimsByLandlordIdQueryHandler> logger)
        {
            _landlordClaimRepository = landlordclaimRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<GetPendingLandlordClaimsDto>> HandleAsync(QueryPendingClaimsByLandlordId query)
        {
            var result = new List<GetPendingLandlordClaimsDto>();

            Expression<Func<LandlordPropertyClaim, bool>> expression = c => c.LandlordId == query.LandlordId &&
                                                                            c.HasJiraTicket == false &&
                                                                            c.Status == PropertyClaimStatusEnum.Pending.GetEnumDescription();
            var claims = await _landlordClaimRepository.Get(expression);
            if (claims is not null && claims.Count() > 0)
            {
                foreach (var claim in claims)
                {
                    result.Add(MapClaim(claim));
                }
            }

            return result;
        }

        private GetPendingLandlordClaimsDto MapClaim(LandlordPropertyClaim claim)
        {
            var result = new GetPendingLandlordClaimsDto
            {
                Id = claim.Id,
                MlsID = claim.MlsID,
                Address = claim.Address,
                City = claim.City,
                Unit = claim.Unit,
                ListingUrl = claim.ListingUrl,
                OwnershipProofs = FileInfoDisplayDto.AdaptFrom(claim.OwnershipProofs)
            };
            return result;
        }

    }
}
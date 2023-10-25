using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Support.Application.Queries;
using Totira.Support.Persistance.Mongo.Util;
using Totira.Support.Persistance.Util;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries.Landlord
{
    public class GetPropertyClaimsDisplayByLandlordIdQueryHandler : IQueryHandler<QueryPropertyClaimsDisplayByLandlordId, GetLandlordClaimsDisplayDto>
    {
        private readonly IRepository<LandlordPropertyClaim, Guid> _claimsRepository;
        private readonly IRepository<LandlordPropertyDisplay, Guid> _propertiesDisplayRepository;

        public GetPropertyClaimsDisplayByLandlordIdQueryHandler(IRepository<LandlordPropertyClaim, Guid> claimsRepository,
                                                                IRepository<LandlordPropertyDisplay, Guid> propertiesDisplayRepository)
        {
            _claimsRepository = claimsRepository;
            _propertiesDisplayRepository = propertiesDisplayRepository;
        }
        public async Task<GetLandlordClaimsDisplayDto> HandleAsync(QueryPropertyClaimsDisplayByLandlordId query)
        {

            IMongoFilter<LandlordPropertyClaim> claimFilter = new MongoFilter<LandlordPropertyClaim>();
            claimFilter.AddCondition(x => x.LandlordId == query.LandlordId);

            var claimCount = await _claimsRepository.GetCountAsync(claimFilter);

            var claims = await _claimsRepository.GetPageAsync(claimFilter, query.PageNumber, query.PageSize, "CreatedOn", true);

            var result = new GetLandlordClaimsDisplayDto
            {
                ClaimedCount = claimCount,
                PublishedCount = GetPropertyCountByStatus(query.LandlordId, PropertyStatusEnum.Published.GetEnumDescription()),
                UnpublishedCount = GetPropertyCountByStatus(query.LandlordId, PropertyStatusEnum.Unpublished.GetEnumDescription()),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                ClaimedProperties = new List<PropertyClaimDisplayDto>()
            };

            foreach (var claim in claims)
            {
                result.ClaimedProperties.Add(MapClaimDisplay(claim));
            }
            return result;
        }

        private long GetPropertyCountByStatus(Guid landlordId, string status)
        {
            long result = 0;

            IMongoFilter<LandlordPropertyDisplay> countFilter = new MongoFilter<LandlordPropertyDisplay>();
            countFilter.AddCondition(x => x.LandlordId == landlordId);
            countFilter.AddCondition(x => x.Status == status);

            return _propertiesDisplayRepository.GetCountAsync(countFilter).Result;
        }

        private PropertyClaimDisplayDto MapClaimDisplay(LandlordPropertyClaim claim)
        {
            var result = new PropertyClaimDisplayDto
            {
                ClaimDate = claim.CreatedOn.ToString( "MMM dd, yyyy"),
                Status = claim.Status,
                Reason = claim.Reason,
                DetailsType = GetClaimDetailType(claim),
                Details = GetClaimDetail(claim)
            };
            return result;
        }

        private string GetClaimDetail(LandlordPropertyClaim claim)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(claim.Address))
            {
                result = ($"{claim.Unit} - {claim.Address}, {claim.City}");
            }
            if (!string.IsNullOrEmpty(claim.ListingUrl))
            {
                result = claim.ListingUrl;
            }
            if (!string.IsNullOrEmpty(claim.MlsID))
            {
                result = claim.MlsID;
            }
            return result;
        }

        private string GetClaimDetailType(LandlordPropertyClaim claim)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(claim.Address))
            {
                result = ClaimDetailTypeEnum.Location.GetEnumDescription();
            }
            if (!string.IsNullOrEmpty(claim.ListingUrl))
            {
                result = ClaimDetailTypeEnum.Url.GetEnumDescription();
            }
            if (!string.IsNullOrEmpty(claim.MlsID))
            {
                result = ClaimDetailTypeEnum.Mls.GetEnumDescription();
            }

            return result;
        }
    }
}

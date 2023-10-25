using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;
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
    public class GetLandlordPropertiesDisplayQueryHandler: IQueryHandler<QueryLandlordPropertiesDisplayByLandlordId, GetLandlordPropertiesDisplayDto>
    {
        private readonly IRepository<LandlordPropertyClaim, Guid> _claimsRepository;
        private readonly IRepository<LandlordPropertyDisplay, Guid> _propertiesDisplayRepository;
        private readonly IRepository<TenantPropertyApplication, Guid> _applicationsRepository;

        public GetLandlordPropertiesDisplayQueryHandler(IRepository<LandlordPropertyClaim, Guid> claimsRepository,
                                                        IRepository<LandlordPropertyDisplay, Guid> propertiesDisplayRepository)
        {
            _claimsRepository = claimsRepository;
            _propertiesDisplayRepository = propertiesDisplayRepository;
        }
        public async Task<GetLandlordPropertiesDisplayDto> HandleAsync(QueryLandlordPropertiesDisplayByLandlordId query)
        {
            var result = new GetLandlordPropertiesDisplayDto
            {
                ClaimedCount = await GetClaimCount(query),
                PublishedCount = GetPropertyCountByStatus(query.LandlordId, PropertyStatusEnum.Published.GetEnumDescription()),
                UnpublishedCount = GetPropertyCountByStatus(query.LandlordId, PropertyStatusEnum.Unpublished.GetEnumDescription()),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                ListedProperties = await GetPropertiesListByStatus(query, PropertyStatusEnum.Published.GetEnumDescription()),
                UnpublishedProperties = await GetPropertiesListByStatus(query, PropertyStatusEnum.Unpublished.GetEnumDescription())
            };

            return result;
        }

        private async Task<long> GetClaimCount(QueryLandlordPropertiesDisplayByLandlordId query)
        {
            IMongoFilter<LandlordPropertyClaim> claimFilter = new MongoFilter<LandlordPropertyClaim>();
            claimFilter.AddCondition(x => x.LandlordId == query.LandlordId);
            var claimCount = await _claimsRepository.GetCountAsync(claimFilter);
            return claimCount;
        }

        private long GetPropertyCountByStatus(Guid landlordId, string status)
        {
            long result = 0;

            IMongoFilter<LandlordPropertyDisplay> countFilter = new MongoFilter<LandlordPropertyDisplay>();
            countFilter.AddCondition(x => x.LandlordId == landlordId);
            countFilter.AddCondition(x => x.Status == status);

            return _propertiesDisplayRepository.GetCountAsync(countFilter).Result;
        }

        private async Task<List<PropertyDisplayDto>> GetPropertiesListByStatus(QueryLandlordPropertiesDisplayByLandlordId query, string status)
        {
            var result = new List<PropertyDisplayDto>();

            var propertiesFilter = GetPropertiesFilter(query, status);
            var properties = await _propertiesDisplayRepository.GetPageAsync(propertiesFilter, 
                                                                             query.PageNumber, 
                                                                             query.PageSize, 
                                                                             query.OrderBy.GetEnumDescription(),
                                                                             query.Descending);
            foreach (var property in properties)
            {
                result.Add(new PropertyDisplayDto()
                {
                    PropertyDisplayId = property.Id,
                    ML_Num = property.MlsId,
                    Location = property.Location,
                    Size = property.Size,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    Price = property.Price,
                    ListingDate = property.AvaillableDate,
                    ApplicationCount = await GetApplicationCount(property.MlsId)
                });
            }
            return result;
        }
        private async Task<long> GetApplicationCount(string mls_num)
        {
            long result = 0;
            IMongoFilter<TenantPropertyApplication> countFilter = new MongoFilter<TenantPropertyApplication>();
            countFilter.AddCondition(x => x.PropertyId == mls_num);
            result = await _applicationsRepository.GetCountAsync(countFilter);
            return result;
        }
        private IMongoFilter<LandlordPropertyDisplay> GetPropertiesFilter(QueryLandlordPropertiesDisplayByLandlordId query, string status)
        {

            IMongoFilter<LandlordPropertyDisplay> propertyFilter = new MongoFilter<LandlordPropertyDisplay>();
            propertyFilter.AddCondition(x => x.LandlordId == query.LandlordId);
            propertyFilter.AddCondition(x => x.Status == status);

            return propertyFilter;
        }
    }
}

using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using X.PagedList;
using System.Collections.Generic;
using System.Linq;
using static Totira.Support.Persistance.IRepository;
using MongoDB.Driver.Core.Authentication;
using System.Linq.Expressions;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries;

public class QueryPropertyHandler : IQueryHandler<QueryProperty, GetPropertyDto>
{
    private readonly IRepository<Property, string> _propertydataRepository;
    public QueryPropertyHandler(IRepository<Property, string> propertydataRepository)
    {
        _propertydataRepository = propertydataRepository;
    }

    public async Task<GetPropertyDto> HandleAsync(QueryProperty query)
    {
        var info = (await _propertydataRepository.Get(p => p.Id != null));
        GetPropertyDto getPropertyDto = new GetPropertyDto();
        getPropertyDto.Count = info.Count();
        getPropertyDto.PageNumber = query.PageNumber;
        getPropertyDto.PageSize = query.PageSize;
        getPropertyDto.SortBy = query.SortBy;
        if (info.Any())
        {
            var paggingData = FilterProperties(info.ToList(), query.QueryFilter);
            getPropertyDto.Properties = SortingProperties(paggingData, query.SortBy).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                                        .Select(item => new PropertyDto()
                                        {
                                            Id = item.Id,
                                            Ml_num = item.Id,
                                            Address = item.Address,
                                            StreetName = item.StreetName,
                                            Location = item.Street + " " + item.StreetName,
                                            ListPrice = item.ListPrice,
                                            OriginalPrice = item.OriginalPrice,
                                            ApproxSquareFootage = item.ApproxSquareFootage,
                                            Area = item.Area,
                                            Bedrooms = item.Bedrooms,
                                            Washrooms = item.Washrooms,
                                        }).ToList();
        }
        return getPropertyDto;
    }

    #region Helper Methods
    List<Property> SortingProperties(List<Property> properties, EnumPropertySortBy? sortBy)
    {
        switch (sortBy)
        {
            case EnumPropertySortBy.Newest:
                properties = properties.OrderByDescending(s => s.CreatedOn).ToList();
                break;
            case EnumPropertySortBy.PriceLowToHigh:
                properties = properties.OrderBy(s => s.ListPrice).ToList();
                break;
            case EnumPropertySortBy.PriceHighToLow:
                properties = properties.OrderByDescending(s => s.ListPrice).ToList();
                break;
            default:
                properties = properties.OrderByDescending(s => s.CreatedOn).ToList();
                break;
        }
        return properties;
    }

    List<Property> FilterProperties(IEnumerable<Property> properties, QueryFilter filter)
    {
        properties = properties.WhereByIf(!string.IsNullOrEmpty(filter.Type),
                                     p => (filter.Type.Trim().ToLower() == "Apartment" ?
                                                              p.TypeOwn1Out.ToLower().Contains("Condo Apt")
                                                           || p.TypeOwn1Out.ToLower().Contains("Co-Op Apt")
                                                           || p.TypeOwn1Out.ToLower().Contains("Comm Element Condo")
                                                           : filter.Type.Trim().ToLower() == "Townhouse" ?
                                                             p.TypeOwn1Out.ToLower().Contains("Condo Townhouse")
                                                             : true))


                         .WhereByIf(filter.MinimumPrice > 0,
                                             p => (p.ListPrice >= filter.MinimumPrice))

                         .WhereByIf(filter.MaximumPrice > 0,
                                     p => (p.ListPrice <= filter.MaximumPrice))

                         .WhereByIf(filter.Bedrooms > 0,
                                     p => (p.Bedrooms == filter.Bedrooms))

                         .WhereByIf(filter.Bathrooms > 0,
                                     p => (p.Washrooms == filter.Bathrooms))

                         .WhereByIf(!string.IsNullOrEmpty(filter.AirConditioning),
                                     p => (p.AirConditioning != null || p.AirConditioning != "None"))

                         .WhereByIf(filter.MinimumSize > 0,
                                             p => (Convert.ToDecimal(p.ApproxSquareFootage) >= filter.MinimumSize))

                         .WhereByIf(filter.MaximumSize > 0,
                                     p => (Convert.ToDecimal(p.ApproxSquareFootage) <= filter.MaximumSize))

                         .WhereByIf(filter.Parking > 0,
                                     p => (p.TotalParkingSpaces <= filter.Parking))

                         .WhereByIf(!string.IsNullOrEmpty(filter.Pool),

                           p => (p.condo.BuildingAmenities.Contains("Indoor Pool") ||
                                                        (p.condo.BuildingAmenities.Contains("Lap Pool")) ||
                                                        (p.condo.BuildingAmenities.Contains("Outdoor Pool"))||
                                                        (p.residential.Pool != null || p.residential.Pool != "None")))

                         .WhereByIf(!string.IsNullOrEmpty(filter.PetsAllowed),
                                   p => (p.condo.PetsPermitted == "Y"))


                         .WhereByIf(!string.IsNullOrEmpty(filter.Furnished),
                                                       p => (p.Furnished == "Y"))

                         .WhereByIf(!string.IsNullOrEmpty(filter.Laundry),
                                                   p => (p.LaundryAccess != null))

                         .WhereByIf(!string.IsNullOrEmpty(filter.Unfurnished),
                                                   p => (p.Furnished == "N"))


                         .WhereByIf(!string.IsNullOrEmpty(filter.WheelchairAccesible),
                                                   p => (p.PhyHandiEquipped == "Y"))

                        .WhereByIf(!string.IsNullOrEmpty(filter.SecuritySystem),
                                                   p => (p.condo.BuildingAmenities.Contains("Security System")))

                        .WhereByIf(!string.IsNullOrEmpty(filter.SecurityGuard),
                                                       p => (p.condo.BuildingAmenities.Contains("Concierge") ||
                                                        (p.condo.BuildingAmenities.Contains("Security Guard"))))


                        .WhereByIf(!string.IsNullOrEmpty(filter.CentralVacuum),
                                                   p => (p.CentralVac == "Y"))


                        .WhereByIf(!string.IsNullOrEmpty(filter.HeatIncluded),
                                                   p => (p.HeatIncluded == "Y"))

                        .WhereByIf(!string.IsNullOrEmpty(filter.HydroIncluded),
                                                   p => (p.HydroIncluded == "Y"))


                        .WhereByIf(!string.IsNullOrEmpty(filter.WaterIncluded),
                                                   p => (p.WaterIncluded == "Y"))

                        .WhereByIf(!string.IsNullOrEmpty(filter.CableIncluded),
                                                   p => (p.CacIncluded == "Y"))

                        .WhereByIf(!string.IsNullOrEmpty(filter.CentralAirConditioning),
                                                   p => (p.CacIncluded == "Y"))

                        .WhereByIf(!string.IsNullOrEmpty(filter.Balcony),
                                                   p => (p.condo.Balcony != null))

                         .WhereByIf(!string.IsNullOrEmpty(filter.GymFitnessCenter),
                                                   p => (p.condo.BuildingAmenities.Contains("Exercise Room") ||
                                                        (p.condo.BuildingAmenities.Contains("Gym"))))

                         .WhereByIf(!string.IsNullOrEmpty(filter.OutdoorSpace),
                                                   p => (p.condo.BuildingAmenities.Contains("Rooftop Deck/Garden")))


                         .WhereByIf(!string.IsNullOrEmpty(filter.RecreationMeetingRoom),
                                                   p => (p.condo.BuildingAmenities.Contains("Party/Meeting Room") ||
                                                        (p.condo.BuildingAmenities.Contains("Games Room")) ||
                                                        (p.condo.BuildingAmenities.Contains("Recreation Room"))))

        ;

        return properties.ToList();
    }

    #endregion

}
public static class QueryExtension
{
    public static IEnumerable<TSource> WhereByIf<TSource>(this IEnumerable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
    {
        if (condition == true)
            return query.AsQueryable().Where(predicate);
        else
            return query;
    }
}


using MongoDB.Driver;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.DTO.Common;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.Persistance.Mongo.Util;
using Totira.Support.Persistance.Util;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries;

public class QueryPropertyHandler : IQueryHandler<QueryProperty, GetPropertyDto>
{
    private readonly IRepository<Property, string> _propertydataRepository;
    private readonly IRepository<PropertyImages, string> _propertyImagesRepository;
    private readonly IS3Handler _s3Handler;

    public QueryPropertyHandler(
        IRepository<Property, string> propertydataRepository,
        IRepository<PropertyImages, string> propertyImagesRepository,
        IS3Handler s3Handler)
    {
        _propertydataRepository = propertydataRepository;
        _propertyImagesRepository = propertyImagesRepository;
        _s3Handler = s3Handler;
    }

    public async Task<GetPropertyDto> HandleAsync(QueryProperty query)
    {
        GetPropertyDto response = new();

        bool descending = query.SortBy switch
        {
            EnumPropertySortBy.Newest => true,
            EnumPropertySortBy.PriceHighToLow => true,
            EnumPropertySortBy.PriceLowToHigh => false,
            _ => true
        };

        IMongoFilter<Property> filter = new MongoFilter<Property>();

        // Use following filters is the correct way to get data due to mongoDB compatibility, some expressions are not compatible with mongoDB linq
        // Less is more
        if (query.Filter is not null)
        {
            var apartmentList = new List<string> { "Condo Apt", "Co-Op Apt", "Comm Element Condo" };
            var houseList = new List<string> { "Att/Row/Twnhouse", "Cottage", "Detached", "Duplex", "Semi-Detached", "Triplex", "Vacand Land" };

            if (query.Filter.MinimumPrice.HasValue)
                filter.AddCondition(property => property.ListPrice >= query.Filter.MinimumPrice);
            if (query.Filter.MaximumPrice.HasValue)
                filter.AddCondition(property => property.ListPrice <= query.Filter.MaximumPrice);
            
            if (!string.IsNullOrWhiteSpace(query.Filter.Type) && query.Filter.Type == "Apartment")
                filter.AddCondition(property => apartmentList.Contains(property.TypeOwn1Out));

            if (!string.IsNullOrWhiteSpace(query.Filter.Type) && query.Filter.Type == "House")
                filter.AddCondition(property => houseList.Contains(property.TypeOwn1Out));

            if (!string.IsNullOrWhiteSpace(query.Filter.Type) && query.Filter.Type == "Townhouse")
                filter.AddCondition(property => property.TypeOwn1Out == "Condo Townhouse");
            if (query.Filter.MinimumSize.HasValue)
                filter.AddCondition(property => property.MinApproxSquareFootage >= query.Filter.MinimumSize);
            if (query.Filter.MaximumSize.HasValue)
                filter.AddCondition(property => property.MaxApproxSquareFootage <= query.Filter.MaximumSize);
            
            if (!string.IsNullOrWhiteSpace(query.Filter.Bedrooms) &&
                query.Filter.Bedrooms != "Any" &&
                int.TryParse(query.Filter.Bedrooms, out int bedrooms))
                filter.AddCondition(property => property.Bedrooms == bedrooms);
            else if (!string.IsNullOrWhiteSpace(query.Filter.Bedrooms) && query.Filter.Bedrooms == "4+")
                filter.AddCondition(property => property.Bedrooms >= 4);

            if (!string.IsNullOrWhiteSpace(query.Filter.Bathrooms) &&
                query.Filter.Bathrooms != "Any" &&
                int.TryParse(query.Filter.Bathrooms, out int bathrooms))
                filter.AddCondition(property => property.Washrooms == bathrooms);
            else if (!string.IsNullOrWhiteSpace(query.Filter.Bathrooms) && query.Filter.Bathrooms == "4+")
                filter.AddCondition(property => property.Washrooms >= 4);

            if (query.Filter.Amenities is not null)
            {
                var poolList = new List<string> { "Outdoor Pool", "Indoor Pool", "Lap Pool" };
                var gymList = new List<string> { "Exercise Room", "Gym" };
                var securityGuardOrConciergeList = new List<string> { "Security Guard", "Concierge" };
                var securitySystemList = new List<string> { "Security System" };
                var outdoorSpaceList = new List<string> { "Rooftop Deck/Garden" };
                var recreatingRoomList = new List<string> { "Party/Meeting Room", "Meeting Room", "Rereation Room", "Games Room" };

                if (query.Filter.Amenities.AirConditioning)
                    filter.AddCondition(property => property.AirConditioning != null && property.AirConditioning != "None");
                
                if (query.Filter.Amenities.Laundry)
                    filter.AddCondition(property => property.LaundryAccess != null && property.LaundryAccess != "None");

                if (query.Filter.Amenities.Pool)
                    filter.AddOrFilters(new List<FilterDefinition<Property>>
                    {
                        Builders<Property>.Filter.AnyIn(property => property.condo.BuildingAmenities, poolList),
                        Builders<Property>.Filter.Where(property => property.residential.Pool != "None" && property.residential.Pool != null)
                    });
                
                if (query.Filter.Amenities.Furnished)
                    filter.AddCondition(property => property.Furnished == "Y");
                if (query.Filter.Amenities.Unfurnished)
                    filter.AddCondition(property => property.Furnished == "N" || property.Furnished == null);
                                            
                if (query.Filter.Amenities.GymOrFitnessCenter)
                    filter.AddAnyIn(property => property.condo.BuildingAmenities, gymList);
                
                if (query.Filter.Amenities.Balcony)
                    filter.AddCondition(property => property.condo.Balcony.Trim() != string.Empty);

                if (query.Filter.Amenities.WheelchairAccesible)
                    filter.AddCondition(property => property.PhyHandiEquipped == "Y");

                if (query.Filter.Amenities.SecurityGuardOrConcierge)
                    filter.AddAnyIn(property => property.condo.BuildingAmenities, securityGuardOrConciergeList);
                
                if (query.Filter.Amenities.SecuritySystem)
                    filter.AddAnyIn(property => property.condo.BuildingAmenities, securitySystemList);

                if (query.Filter.Amenities.OutdoorSpace)
                    filter.AddAnyIn(property => property.condo.BuildingAmenities, outdoorSpaceList);

                if (query.Filter.Amenities.RecreationOrMeetingRoom)
                    filter.AddAnyIn(property => property.condo.BuildingAmenities, recreatingRoomList);

                if (query.Filter.Amenities.CentralVacuum)
                    filter.AddCondition(property => property.CentralVac == "Y");
            }

            if (query.Filter.Utilities is not null)
            {
                if (query.Filter.Utilities.HeatIncluded)
                    filter.AddCondition(property => property.HeatIncluded == "Y");
                if (query.Filter.Utilities.HydroIncluded)
                    filter.AddCondition(property => property.HydroIncluded == "Y");
                if (query.Filter.Utilities.WaterIncluded)
                    filter.AddCondition(property => property.WaterIncluded == "Y");
                if (query.Filter.Utilities.CableIncluded)
                    filter.AddCondition(property => property.CableTVIncluded == "Y");
                if (query.Filter.Utilities.CentralAirConditioning)
                    filter.AddCondition(property => property.CacIncluded == "Y");
            }
            
            if (!string.IsNullOrWhiteSpace(query.Filter.Parking) &&
                query.Filter.Parking != "Any" &&
                int.TryParse(query.Filter.Parking, out int parkingSpaces))
                filter.AddCondition(property => property.ParkingSpaces == parkingSpaces);
            else if (query.Filter.Parking == "4+")
                filter.AddCondition(property => property.ParkingSpaces >= 4);

            if (query.Filter.PetsAllowed)
                filter.AddCondition(property => property.condo.PetsPermitted == "Y");
            else
                filter.AddCondition(property => property.condo.PetsPermitted == "Restrict"
                                             || property.condo.PetsPermitted == "Restricted"
                                             || property.condo.PetsPermitted == "N"
                                             || property.condo.PetsPermitted == string.Empty
                                             || property.condo.PetsPermitted == null);


            if (!string.IsNullOrWhiteSpace(query.Filter.Area))
                filter.AddCondition(property => property.Area.Contains(query.Filter.Area));
            if (!string.IsNullOrWhiteSpace(query.Filter.Province))
                filter.AddCondition(property => property.Province.Contains(query.Filter.Province));
        }

        // Use it when you have to debug if any condition can be allowed by Amazon's DocumentDb
        // if (filter.HasAnyExpr())
        //     throw new Exception("Filter contains $expr. DocumentDb doesn't allow $expr sentence.");

        IEnumerable<Property> data = Enumerable.Empty<Property>();

        if (query.SortBy == EnumPropertySortBy.Newest)
            data = await _propertydataRepository.GetPageAsync(filter, query.PageNumber, query.PageSize, "CreatedOn", descending);

        if (query.SortBy == EnumPropertySortBy.PriceLowToHigh ||
            query.SortBy == EnumPropertySortBy.PriceHighToLow)
            data = await _propertydataRepository.GetPageAsync(filter, query.PageNumber, query.PageSize, "ListPrice", descending);
        
        response.SortBy = query.SortBy;
        response.PageNumber = query.PageNumber;
        response.PageSize = query.PageSize;
        response.Count = await _propertydataRepository.GetCountAsync(filter);
        response.Properties = data
            .Select(x => new PropertyDto()
            {
                Id = x.Id,
                Ml_num = x.Id,
                Address = x.Address,
                StreetName = x.StreetName,
                Location = x.Street + " " + x.StreetName,
                ListPrice = x.ListPrice,
                OriginalPrice = x.OriginalPrice,
                ApproxSquareFootage = x.ApproxSquareFootage,
                Area = x.Area,
                Bedrooms = x.Bedrooms,
                Washrooms = x.Washrooms,
            })
            .ToList();

        foreach (var property in response.Properties)
        {
            var photos = await _propertyImagesRepository.GetByIdAsync(property.Id);
            var photo = new PropertyImageDto();

            if (photos is not null && photos.Propertyimages is not null && photos.Propertyimages.Any())
            {
                var first = photos.Propertyimages.First(x => x.ImageOrder == 1);
                photo.FileUrl = _s3Handler.GetPreSignedUrl(first.S3KeyName);
                photo.ContentType = first.ContentType;
            }

            property.Photo = photo;
        }
        
        return response;
    }

}

static class FilterExtensions
{
    /// <summary>
    /// Adds an Or filter using a list of <see cref="FilterDefinition{T}"/> filters
    /// </summary>
    /// <param name="filter">Mongo filter</param>
    /// <param name="filters">List of filter definitions</param>
    public static void AddOrFilters(this IMongoFilter<Property> filter, IEnumerable<FilterDefinition<Property>> filters)
    {
        var builder = new MongoFilter<Property>();
        var orFilters = Builders<Property>.Filter.Or(filters);
        builder.SetFilter(orFilters);
        filter.AddFilter(builder);
    }
}
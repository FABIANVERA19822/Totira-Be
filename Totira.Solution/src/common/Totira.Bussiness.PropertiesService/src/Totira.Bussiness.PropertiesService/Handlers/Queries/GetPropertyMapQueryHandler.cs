using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries;

public class GetPropertyMapQueryHandler : IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>>
{
    private readonly IRepository<Property, string> _propertyRepository;
    private readonly IQueryHandler<QueryProperty, GetPropertyDto> _queryPropertyHandler;

    public GetPropertyMapQueryHandler(
        IRepository<Property, string> propertyRepository,
        IQueryHandler<QueryProperty, GetPropertyDto> queryPropertyHandler)
    {
        _propertyRepository = propertyRepository;
        _queryPropertyHandler = queryPropertyHandler;
    }

    public async Task<IEnumerable<GetPropertyMapDto>> HandleAsync(QueryPropertyMap query)
    {
        var response = new List<GetPropertyMapDto>();
        var result = await _queryPropertyHandler.HandleAsync(query);

        var properties = await _propertyRepository.GetManyByIds(result.Properties.Select(x => x.Id));

        response = properties
            .Select(property => new GetPropertyMapDto
            {
                Id = property.Id,
                Latitude = property.Latitude.ToString(),
                Longitude = property.Longitude.ToString(),
                Info = new GetPropertyMapInfoDto
                {
                    Area = property.Area,
                    ApproxSquareFootage = property.ApproxSquareFootage,
                    Bathrooms = (int)property.Washrooms,
                    Bedrooms = (int)property.Bedrooms,
                    Price = property.ListPrice,
                    StreetName = property.StreetName,
                }
            })
            .ToList();

        foreach (var item in result.Properties)
        {
            var property = response.Find(x => x.Id == item.Id);
            if (property is not null && property.Info is not null)
                property.Info.Photo = item.Photo;
        }

        return response;
    }
}

using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries;

public class GetPropertyApplicationDetailQueryHandler : IQueryHandler<QueryPropertyApplicationDetail, GetPropertyApplicationDetailDto>
{
    private readonly IRepository<Property, string> _propertyRepository;
    private readonly IRepository<PropertyImages, string> _propertyImageRepository;
    private readonly ILogger<GetPropertyApplicationDetailQueryHandler> _logger;
    private readonly IS3Handler _s3Handler;

    public GetPropertyApplicationDetailQueryHandler(
        IRepository<Property, string> propertyRepository,
        IRepository<PropertyImages, string> propertyImageRepository,
        ILogger<GetPropertyApplicationDetailQueryHandler> logger,
        IS3Handler s3Handler)
    {
        _propertyRepository = propertyRepository;
        _propertyImageRepository = propertyImageRepository;
        _logger = logger;
        _s3Handler = s3Handler;
    }

    public async Task<GetPropertyApplicationDetailDto> HandleAsync(QueryPropertyApplicationDetail query)
    {
        _logger.LogDebug("Getting property by PropertyId");
        var property = await _propertyRepository.GetByIdAsync(query.PropertyId);
        if (property is null)
        {
            _logger.LogDebug("Property not found.");
            return null!;
        }

        var response = new GetPropertyApplicationDetailDto()
        {
            Area = property.Area,
            Address = property.Address,
            AmountFt2 = property.ApproxSquareFootage,
            AmountBeds = (int)property.Bedrooms,
            AmountBaths = (int)property.Washrooms,
            AmountParkingSpaces = (int)property.ParkingSpaces,
            PropertyFronting = property.residential.FrontingOnNSEW,
            Pets = property.condo.PetsPermitted == "Y",
        };

        _logger.LogDebug("Response builded!");

        var images = await _propertyImageRepository.GetByIdAsync(query.PropertyId);
        if (images is not null
         && images.Propertyimages is not null
         && images.Propertyimages.Any(x => x.ImageOrder == 1))
        {
            _logger.LogDebug("Getting images for Property {id}", query.PropertyId);
            var image = images.Propertyimages.First(x => x.ImageOrder == 1);
            response.Image = new()
            {
                FileUrl = _s3Handler.GetPreSignedUrl(image.S3KeyName),
                ContentType = image.ContentType
            };
        }

        return response;
    }
}

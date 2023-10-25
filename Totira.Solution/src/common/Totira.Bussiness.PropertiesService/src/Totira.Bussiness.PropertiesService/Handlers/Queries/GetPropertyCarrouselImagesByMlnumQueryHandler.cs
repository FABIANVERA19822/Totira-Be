
using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries
{
    public class GetPropertyCarrouselImagesByMlnumQueryHandler : IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto>
    {
        private readonly IRepository<PropertyImages, string> _propertyImageRepository;
        private readonly ILogger<GetPropertyCarrouselImagesByMlnumQueryHandler> _logger;
        private readonly IS3Handler _s3Handler;
        public GetPropertyCarrouselImagesByMlnumQueryHandler(IRepository<PropertyImages, string> propertyImageRepository, ILogger<GetPropertyCarrouselImagesByMlnumQueryHandler> logger, IS3Handler s3Handler)
        {
            _logger = logger;
            _propertyImageRepository = propertyImageRepository;
            _s3Handler = s3Handler;
        }

        public async Task<GetPropertyCarrouselImagesDto> HandleAsync(QueryPropertyCarrouselImagesByMl_num query)
        {
           var result = new GetPropertyCarrouselImagesDto();
           _logger.LogInformation("Getting data from propertyImageRepository");
           var propertyImage = await _propertyImageRepository.GetByIdAsync(query.Ml_num);

            if (propertyImage != null && propertyImage.Propertyimages!=null)
            {
                _logger.LogInformation("Getting Images Url from S3 bucket");
                foreach (var image in propertyImage.Propertyimages)
                {
                    var ImageUrl = _s3Handler.GetPreSignedUrl(image.S3KeyName);
                    result.CarrouselImages.Add(new DTO.PropertyImage() { Url = ImageUrl });
                }
            }
            return result; 
        }
    }
}

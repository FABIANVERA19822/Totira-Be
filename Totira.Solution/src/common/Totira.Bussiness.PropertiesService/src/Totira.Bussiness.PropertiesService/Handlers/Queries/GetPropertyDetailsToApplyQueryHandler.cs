using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Bussiness.PropertiesService.Handlers.Commands;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.Persistance;
using static Totira.Bussiness.PropertiesService.DTO.GetPropertyDetailstoApplyDto;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries
{
    public class GetPropertyDetailsToApplyQueryHandler : IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto>
    {
        private readonly ILogger<GetPropertyDetailsToApplyQueryHandler> _logger;
        private readonly IRepository<Property, string> _propertydataRepository;
        private readonly IRepository<PropertyImages, string> _propertyImageRepository;
        private readonly IS3Handler _s3Handler;

        public GetPropertyDetailsToApplyQueryHandler(ILogger<GetPropertyDetailsToApplyQueryHandler> logger, IRepository<Property, string> propertydataRepository, IRepository<PropertyImages, string> propertyImageRepository, IS3Handler s3Handler)
        {
            _propertydataRepository = propertydataRepository;
            _logger = logger;
            _propertyImageRepository = propertyImageRepository;
            _s3Handler = s3Handler;
        }


        public async Task<GetPropertyDetailstoApplyDto> HandleAsync(QueryPropertyDetailsToApply query)
        {
            Expression<Func<Property, bool>> expression = (p => p.Id == query.PropertyId);
            _logger.LogInformation("Getting data from propertyRepository");
            var info = (await _propertydataRepository.Get(expression)).FirstOrDefault();

            PropertyMainImage mainImage = new PropertyMainImage(string.Empty, string.Empty, string.Empty);
            
            _logger.LogInformation("Getting data from propertyImageRepository");

            var propertyImage = await _propertyImageRepository.GetByIdAsync(query.PropertyId);


            if (propertyImage != null)
            {
                _logger.LogInformation("Getting Image Url from S3 bucket");
                var ImageUrl = _s3Handler.GetPreSignedUrl(propertyImage.Propertyimages.FirstOrDefault().S3KeyName);

                 mainImage = new PropertyMainImage(propertyImage.Propertyimages.FirstOrDefault().S3KeyName, propertyImage.Propertyimages.FirstOrDefault().ContentType, ImageUrl);
            } 

            var result =
                info != null ?
                    new GetPropertyDetailstoApplyDto(mainImage, info.Id, info.Area, info.Address, info.ApproxSquareFootage, info.Bedrooms, info.Washrooms, info.ParkingSpaces,info.condo.PetsPermitted,info.ListPrice ) :
                    new GetPropertyDetailstoApplyDto();

            return result;
        }
    }
}

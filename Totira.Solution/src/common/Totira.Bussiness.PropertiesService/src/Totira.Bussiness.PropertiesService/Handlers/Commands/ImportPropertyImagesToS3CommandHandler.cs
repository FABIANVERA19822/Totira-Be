using CrestApps.RetsSdk.Models;
using CrestApps.RetsSdk.Services;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Commands;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Commands
{
    public class ImportPropertyImagesToS3CommandHandler : IMessageHandler<ImportPropertyImagesToS3Command>
    {

        private readonly ILogger<ImportPropertyImagesToS3CommandHandler> _logger;
        private readonly IRetsClient _client;
        private readonly IRepository<PropertyImages, string> _propertyImageRepository;
        private readonly IS3Handler _s3Handler;
     
        public ImportPropertyImagesToS3CommandHandler(
             IRepository<Property, string> propertydataRepository,
             ILogger<ImportPropertyImagesToS3CommandHandler> logger, IRetsClient client,
             IS3Handler s3Handler,
             IRepository<PropertyImages, string> propertyImageRepository
            )

        {
           
            _logger = logger;
            _client = client;
            _propertyImageRepository = propertyImageRepository;
            _s3Handler = s3Handler;
        }
       public async Task HandleAsync(IContext context, ImportPropertyImagesToS3Command command)
        {
            await _client.Connect();

            List<PhotoIdAlpha> photoIds = new List<PhotoIdAlpha>() { new PhotoIdAlpha(command.PropertyId) };

            IEnumerable<FileObject> propertyImages = await _client.GetObjectAlpha("Property", "Photo", photoIds, false);
            await _client.Disconnect();

            int imageOrder = 0;
            foreach (var image in propertyImages)
            {
                imageOrder++;
                var memoryStream = new MemoryStream();

                image.Content.CopyTo(memoryStream);

                byte[] byteArray = memoryStream.ToArray();

                using var stream = new MemoryStream(byteArray!);
                var keyName = GetFormattedKeyName(
                        propertyId: command.PropertyId,
                        fileName: command.PropertyId + imageOrder.ToString());

                _logger.LogInformation($"Currently trying to upload file with key {keyName}");
                await _s3Handler.UploadSMemorySingleFileAsync(keyName, image.ContentType.MediaType, stream);
                var propertyImageUploadedEvent = new PropertyImageUploadedEvent(command.PropertyId);
            }
            SavePropertyImagesToDB(propertyImages, command.PropertyId);



        }
        private static string GetFormattedKeyName(string propertyId, string fileName)
      => $"{propertyId}/{fileName}";

        public async void SavePropertyImagesToDB(IEnumerable<FileObject> propertyImages, string propertyId)
        {
            PropertyImages PropertImages = new();
            PropertImages.Id = propertyId;
            int imageOrder = 0;
            foreach (FileObject file in propertyImages)
            {
                imageOrder++;
                PropertyImage propertyImage = new PropertyImage()
                {
                    ContentType = file.ContentType.MediaType,
                    S3KeyName = propertyId +"/"+ propertyId+ imageOrder.ToString(),
                    ImageOrder = imageOrder,
                    
                };
                
                PropertImages.Propertyimages.Add(propertyImage);
               
            }
            await _propertyImageRepository.Add(PropertImages);
        }

       
    }
}
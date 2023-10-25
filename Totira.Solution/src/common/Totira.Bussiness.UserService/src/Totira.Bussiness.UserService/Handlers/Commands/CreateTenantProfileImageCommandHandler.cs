
namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using Microsoft.Extensions.Logging;
    using System.IO;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Events;
    using Totira.Support.Application.Messages;
    using Totira.Support.CommonLibrary.Interfaces;
    using Totira.Support.TransactionalOutbox;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;
    public class CreateTenantProfileImageCommandHandler : IMessageHandler<CreateTenantProfileImageCommand>
    {
        private readonly IRepository<TenantProfileImage, Guid> _tenantProfileImageRepository;
        private readonly IS3Handler _s3Handler;
        private readonly ILogger<CreateTenantProfileImageCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantProfileImageCommandHandler(
            IRepository<TenantProfileImage, Guid> tenantPersonalInformationRepository, IS3Handler s3Handler,
            ILogger<CreateTenantProfileImageCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantProfileImageRepository = tenantPersonalInformationRepository;
            _s3Handler = s3Handler;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, CreateTenantProfileImageCommand command)
        {
            _logger.LogDebug($"creating tenant profile image with id {command.TenantId}");

            //validate if data exists
            var tenantProfileImage = new TenantProfileImage()
            {
                Id = command.TenantId,
                FileName = command.File.FileName,
                ContentType = command.File.ContentType
            };

            var actualData = await _tenantProfileImageRepository.GetByIdAsync(tenantProfileImage.Id);

            if (actualData is not null)
            {
                await _tenantProfileImageRepository.Update(tenantProfileImage);
            }
            else
            {
                await _tenantProfileImageRepository.Add(tenantProfileImage);
            }

            using var stream = new MemoryStream(command.File.Data!);
            var keyName = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    fileName: tenantProfileImage.FileName);

            _logger.LogInformation($"Currently trying to upload file with key {keyName}");

            await _s3Handler.UploadSMemorySingleFileAsync(keyName, command.File.ContentType, stream);

            var tenantProfileImageCreatedEvent = new TenantProfileImageCreatedEvent(tenantProfileImage.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantProfileImageCreatedEvent);
        }

        private static string GetFormattedKeyName(string tenantId, string fileName)
            => $"{tenantId}/{fileName}";
    }
}

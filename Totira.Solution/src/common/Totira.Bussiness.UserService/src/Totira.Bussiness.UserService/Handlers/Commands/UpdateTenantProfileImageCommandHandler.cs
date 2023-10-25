﻿using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantProfileImageCommandHandler : IMessageHandler<UpdateTenantProfileImageCommand>
    {
        private readonly IRepository<TenantProfileImage, Guid> _tenantProfileImageRepository;
        private readonly IS3Handler _s3Handler;
        private readonly ILogger<UpdateTenantProfileImageCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public UpdateTenantProfileImageCommandHandler(
                    IRepository<TenantProfileImage,
                    Guid> tenantPersonalInformationRepository, 
                    IS3Handler s3Handler,
                    ILogger<UpdateTenantProfileImageCommandHandler> logger,
                    IContextFactory contextFactory,
                    IMessageService messageService)
        {
            _tenantProfileImageRepository = tenantPersonalInformationRepository;
            _s3Handler = s3Handler;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, UpdateTenantProfileImageCommand command)
        {
            _logger.LogDebug($"updating tenant profile image with id {command.TenantId}");

            //validate if data exists
            var tenantProfileImage = new TenantProfileImage()
            {
                Id = command.TenantId,
                FileName = command.File != null ? command.File.FileName : string.Empty,
                ContentType = command.File != null ? command.File.ContentType : string.Empty
            };

            var actualData = await _tenantProfileImageRepository.GetByIdAsync(tenantProfileImage.Id);

            if (actualData is not null && command.File != null)
            {
                var keyName1 = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    fileName: actualData.FileName);
                await _s3Handler.DeleteObjectAsync(keyName1);
                await _tenantProfileImageRepository.Update(tenantProfileImage);

                using var stream = new MemoryStream(command.File.Data!);
                var keyName = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    fileName: tenantProfileImage.FileName);
                await _s3Handler.UploadSMemorySingleFileAsync(keyName, command.File.ContentType, stream);
            }
            else
            {
                var keyName = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    fileName: tenantProfileImage.FileName);
                await _s3Handler.DeleteObjectAsync(keyName);
                await _tenantProfileImageRepository.Delete(tenantProfileImage);

            }


            var userUpdatedEvent = new TenantProfileImageUpdatedEvent(command.TenantId);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, userUpdatedEvent);
        }
        private static string GetFormattedKeyName(string tenantId, string fileName)
            => $"{tenantId}/{fileName}";
    }
}

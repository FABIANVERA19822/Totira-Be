using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Events.Landlord.CreatedEvents;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create
{
    public class CreateLandlordIdentityHandler : IMessageHandler<CreateLandlordIdentityCommand>
    {
        private readonly IRepository<LandlordIdentityInformation, Guid> _landlordIdentityInformationRepository;
        private readonly ILogger<CreateLandlordIdentityHandler> _logger;
        private readonly IS3Handler _s3Handler;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreateLandlordIdentityHandler(
            IRepository<LandlordIdentityInformation, Guid> landlordIdentityInformationRepository,
            ILogger<CreateLandlordIdentityHandler> logger,
            IS3Handler s3Handler,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _landlordIdentityInformationRepository = landlordIdentityInformationRepository;
            _logger = logger;
            _s3Handler = s3Handler;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, Either<Exception, CreateLandlordIdentityCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogInformation("creating landlord identity information with id {landlordId}", cmd.LandlordId);
                Guid? messageOutboxId = null;

                var identityProofs = new List<Domain.Common.File>();

                _logger.LogInformation("Starts to iterate files to be uploaded.");

                foreach (var file in cmd.IdentityProofs)
                {
                    var key = GetFormattedKeyName(
                        landlordId: cmd.LandlordId.ToString(),
                        fileName: file.FileName);

                    var isUploaded = await UploadFileToS3Async(key, file.ContentType, file.Data!);

                    if (isUploaded)
                    {
                        var fileInfo = Domain.Common.File.Create(file.FileName, key, file.ContentType, file.Size);
                        identityProofs.Add(fileInfo);
                        _logger.LogInformation("File: [name: {fileName}, key: {key}] added to file info list.", file.FileName, key);
                    }
                }

                var landlordIdentity = new LandlordIdentityInformation
                {
                    Id = cmd.LandlordId,
                    LandlordId = cmd.LandlordId,
                    PhoneNumber = new ContactInformationPhoneNumber(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                    IdentityProofs = identityProofs
                };

                await _landlordIdentityInformationRepository.Add(landlordIdentity);

                var userCreatedEvent = new LandlordIdentityCreatedEvent(landlordIdentity.Id, true);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
            }, async ex =>
            {
                var userCreatedEvent = new LandlordIdentityCreatedEvent(Guid.Empty, false);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
                throw ex;
            });
        }
        private async Task<bool> UploadFileToS3Async(string key, string contentType, byte[] data)
        {
            using var ms = new MemoryStream(data);
            var response = await _s3Handler.UploadSMemorySingleFileAsync(key, contentType, ms);
            return response;
        }
        private static string GetFormattedKeyName(string landlordId, string fileName)
            => $"{landlordId}/identity/{fileName}";
    }
}

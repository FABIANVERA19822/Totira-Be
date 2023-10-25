using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events.Landlord.CreatedEvents;
using Totira.Bussiness.UserService.Extensions;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create
{
    internal class CreatePropertyClaimsFromLandlordHandler : IMessageHandler<CreatePropertyClaimsFromLandlordCommand>
    {
        private readonly IRepository<LandlordPropertyClaim, Guid> _propertyClaimRepository;
        private readonly ILogger<CreatePropertyClaimsFromLandlordHandler> _logger;
        private readonly IS3Handler _s3Handler;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreatePropertyClaimsFromLandlordHandler(
            IRepository<LandlordPropertyClaim, Guid> propertyClaimRepository,
            ILogger<CreatePropertyClaimsFromLandlordHandler> logger,
            IS3Handler s3Handler,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _propertyClaimRepository = propertyClaimRepository;
            _logger = logger;
            _s3Handler = s3Handler;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, Either<Exception, CreatePropertyClaimsFromLandlordCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogInformation("creating landlord identity information with id {landlordId}", cmd.LandlordId);
            Guid? messageOutboxId = null;

            _logger.LogInformation("Starts to iterate files to be uploaded.");

            foreach (var claimDto in cmd.ClaimDetails)
            {
                var landlordPropertyClaim = await CreateNewClaim(cmd, claimDto);

                foreach (var proof in claimDto.OwnershipProofs)
                {
                    var key = GetFormattedKeyName(
                    landlordId: cmd.LandlordId.ToString(),
                    claimId: landlordPropertyClaim.Id.ToString(),
                    fileName: proof.FileName);

                    var isUploaded = await UploadFileToS3Async(key, proof.ContentType, proof.Data!);

                    if (isUploaded)
                    {
                        var fileInfo = Domain.Common.File.Create(proof.FileName, key, proof.ContentType, proof.Size);
                        landlordPropertyClaim.OwnershipProofs.Add(fileInfo);
                        _logger.LogInformation("File: [name: {fileName}, key: {key}] added to file info list.", proof.FileName, key);
                    }

                }
                await _propertyClaimRepository.Add(landlordPropertyClaim);
            }

            var userCreatedEvent = new LandlordPropertyClaimsCreatedEvent(cmd.LandlordId, true);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
            }, async ex =>
            {
                var userCreatedEvent = new LandlordPropertyClaimsCreatedEvent(Guid.Empty,false);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
                throw ex;
            });
        }

        private async Task<LandlordPropertyClaim> CreateNewClaim(CreatePropertyClaimsFromLandlordCommand command, PropertyClaimDetailsDto claim)
        {
            var result = new LandlordPropertyClaim
            {
                Id = Guid.NewGuid(),
                LandlordId = command.LandlordId,
                Status = PropertyClaimStatusEnum.Pending.GetEnumDescription(),
                Role = command.Role,
                Address = claim.Address,
                City = claim.City,
                Unit = claim.Unit,
                ListingUrl = claim.ListingUrl,
                MlsID = claim.MlsID,
                OwnershipProofs = new List<Domain.Common.File>(),
                CreatedOn = DateTimeOffset.Now
            };
            return result;
        }

        private async Task<bool> UploadFileToS3Async(string key, string contentType, byte[] data)
        {
            using var ms = new MemoryStream(data);
            var response = await _s3Handler.UploadSMemorySingleFileAsync(key, contentType, ms);
            return response;
        }
        private static string GetFormattedKeyName(string landlordId,string claimId,string fileName)
            => $"{landlordId}/{claimId}/{fileName}";


    }
}

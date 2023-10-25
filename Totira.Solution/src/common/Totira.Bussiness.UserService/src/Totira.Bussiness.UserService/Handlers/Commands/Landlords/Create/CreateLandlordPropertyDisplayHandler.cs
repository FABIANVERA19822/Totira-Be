using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.PropertyService;
using Totira.Bussiness.UserService.Events.Landlord.CreatedEvents;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.Persistance;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create
{
    internal class CreateLandlordPropertyDisplayHandler : BaseMessageHandler<CreateLandlordPropertyDisplayCommand, LandlordPropertyDisplayCreatedEvent>
    {
        private readonly IRepository<LandlordPropertyDisplay, Guid> _propertiesRepository;
        private readonly IRepository<LandlordPropertyClaim, Guid> _claimsRepository;
        private readonly ILogger<CreateLandlordPropertyDisplayHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;

        public CreateLandlordPropertyDisplayHandler(
            IRepository<LandlordPropertyDisplay, Guid> propertiesRepository, 
            IRepository<LandlordPropertyClaim, Guid> claimsRepository, 
            ILogger<CreateLandlordPropertyDisplayHandler> logger,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            IContextFactory contextFactory, 
            IMessageService messageService) : base(logger, contextFactory, messageService)
        {
            _propertiesRepository = propertiesRepository;
            _claimsRepository = claimsRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
        }

        protected override async Task<LandlordPropertyDisplayCreatedEvent> Process(IContext context, CreateLandlordPropertyDisplayCommand command)
        {
            var claim = await _claimsRepository.GetByIdAsync(command.ClaimId);

            GetPropertyDetailsDto propertyInfo = await GetPropertyInfo(command);

            if (propertyInfo == null)
                    throw new Exception($"Property with id {command.MLSId} was not found.");

            LandlordPropertyDisplay newLandlordProperty = MapPropertyDetailsToDisplay(propertyInfo, claim);

            _propertiesRepository.Add(newLandlordProperty);
            throw new NotImplementedException();
        }

        private LandlordPropertyDisplay MapPropertyDetailsToDisplay(GetPropertyDetailsDto propertyInfo, LandlordPropertyClaim claim)
        {
            LandlordPropertyDisplay result = new LandlordPropertyDisplay
            {
                Id = claim.Id,
                LandlordId = claim.LandlordId,
                Location = propertyInfo.Address,
                Size = propertyInfo.ApproxSquareFootage,
                Bedrooms = (int)Math.Truncate(propertyInfo.Bedrooms),
                Bathrooms = (int)Math.Truncate(propertyInfo.Washrooms),
                Price = (int)Math.Truncate(propertyInfo.ListPrice),
                AvaillableDate = propertyInfo.ListingEntryDate.Value.ToString("MMM d, yyyy"),
                Status = propertyInfo.Status
            };

            return result;
        }

        private async Task<GetPropertyDetailsDto> GetPropertyInfo(CreateLandlordPropertyDisplayCommand command)
        {
            GetPropertyDetailsDto result = new GetPropertyDetailsDto();

            result = (await _queryRestClient.GetAsync<GetPropertyDetailsDto>($"{_restClientOptions.Properties}/property/propertydata/{command.MLSId}")).Content;

            return result;
        }
    }
}

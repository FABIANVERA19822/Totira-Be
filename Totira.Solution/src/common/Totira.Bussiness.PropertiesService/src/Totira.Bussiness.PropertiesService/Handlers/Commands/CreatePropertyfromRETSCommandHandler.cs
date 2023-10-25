namespace Totira.Bussiness.PropertiesService.Handlers.Commands
{
    using AutoMapper;
    using CrestApps.RetsSdk.Models;
    using CrestApps.RetsSdk.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Text;
    using Totira.Bussiness.PropertiesService.Commands;
    using Totira.Bussiness.PropertiesService.Domain;
    using Totira.Bussiness.PropertiesService.DTO.ThirdpartyService;
    using Totira.Bussiness.PropertiesService.Enums;
    using Totira.Bussiness.PropertiesService.Events;
    using Totira.Support.Api.Connection;
    using Totira.Support.Api.Options;
    using Totira.Support.Application.Messages;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;

    public class CreatePropertyfromRETSCommandHandler : IMessageHandler<CreatePropertyfromRETSCommand>
    {

        private readonly ILogger<CreatePropertyfromRETSCommandHandler> _logger;
        private readonly IRepository<Property, string> _propertydataRepository;
        private readonly IRetsClient _client;
        private readonly IMapper _mapper;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        public CreatePropertyfromRETSCommandHandler(
            IRepository<Property, string> propertydataRepository,
            ILogger<CreatePropertyfromRETSCommandHandler> logger,
            IRetsClient client,
            IMapper mapper,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions
            )

        {
            _propertydataRepository = propertydataRepository;
            _logger = logger;
            _client = client;
            _mapper = mapper;
            _queryRestClient = queryRestClient; 
            _restClientOptions = restClientOptions.Value;
             
        }

        public async Task HandleAsync(IContext context, CreatePropertyfromRETSCommand message)
        {
            try
            {
                _logger.LogInformation($"CreatePropertyfromRETS has started with propertyType: {message.PropertyType}");
                var info = await SearchRequest(message.PropertyType);
                foreach (SearchResultRow row in info.GetRows())
                {
                    var propertyCommand = _mapper.Map<Property>(row);
                    propertyCommand.PropertyType = message.PropertyType;

                    var propertyResidentialCommand = _mapper.Map<Residential>(row);
                    var propertyCondoCommand = _mapper.Map<Condo>(row);

                    propertyCommand.residential = propertyResidentialCommand;
                    propertyCommand.condo = propertyCondoCommand;

                    #region Coordinates From Google Maps TOT-4139
                    _logger.LogInformation("CreatePropertyfromRETS - Getting coordinates from Google Maps ThirdPartyIntegration started");
                    
                    StringBuilder address = new StringBuilder();
                    address.Append(propertyCommand.Address);
                    address.Append($" {propertyCommand.Area}");
                    address.Append($" {propertyCommand.Province}");
                    address.Append(" Canada");

                    _logger.LogInformation($"CreatePropertyfromRETS - Getting coordinates from Google Maps ThirdPartyIntegration url _restClientOptions: {_restClientOptions.ThirdPartyIntegration}");
                    var point = await _queryRestClient.GetAsync<GetPropertyLongitudeAndLatitudeDto>($"{_restClientOptions.ThirdPartyIntegration}/Location/propertylocation/{address}");

                    _logger.LogInformation("CreatePropertyfromRETS - Getting coordinates from Google Maps ThirdPartyIntegration finished");
                    propertyCommand.Latitude = point.Content.Latitude;
                    propertyCommand.Longitude = point.Content.Longitude;
                    #endregion

                    if (await _propertydataRepository.GetByIdAsync(propertyCommand.Id) is null)
                    {
                        await _propertydataRepository.Add(propertyCommand);
                    }else
                    {
                        await _propertydataRepository.Update(propertyCommand);
                    }

                   
                }
                var propertyCreatedEvent = new PropertyCreatedEvent(message.PropertyType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreatePropertyfromRETSCommandHandler process with PropertyType: {message.PropertyType} failed, reason: {ex.Message}, {ex.InnerException}");
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }
        }

        //private async Task<QueryResponse<GetPropertyLongitudeAndLatitudeDto>> GetLocationFromThirdParty(Property propertyCommand)
        //{
        //    StringBuilder address = new StringBuilder();
        //    address.Append(propertyCommand.Address);
        //    address.Append($" {propertyCommand.Area}");
        //    address.Append($" {propertyCommand.Province}");
        //    address.Append(" Canada");

        //    return await _queryRestClient.GetAsync<GetPropertyLongitudeAndLatitudeDto>($"{_restClientOptions.ThirdPartyIntegration}/Location/propertylocation/{address}");
        //}
        private async Task<SearchResult> SearchRequest(string classname)
        {
            try
            {
                await _client.Connect();

                SearchRequest searchRequest = new SearchRequest("Property", classname);
                searchRequest.ParameterGroup.AddParameter(new QueryParameter(ParametersSearchProperty.List_price.GetEnumDescription(), "1+"));
                searchRequest.AddColumns(searchRequest.GetColumns());
                searchRequest.StandardNames = (int)ParametersSearchProperty.StandardNames;
                searchRequest.Limit = (int)ParametersSearchProperty.Limit;

                SearchResult result = await _client.Search(searchRequest);

                await _client.Disconnect();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreatePropertyfromRETSCommandHandler - SearchRequest process with PropertyType: {classname} failed, reason: {ex.Message}, {ex.InnerException}");
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }

        }
    }
}

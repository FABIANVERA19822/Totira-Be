using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.DTO.PropertyService;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService;
public class GetPropertyApplicationRequestDetailQueryHandler : IQueryHandler<QueryPropertyApplicationDetail, PropertyApplicationRequestDetailDto>
{
    private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
    private readonly ILogger<GetPropertyApplicationRequestDetailQueryHandler> _logger;
    private readonly IQueryRestClient _queryRestClient;
    private readonly RestClientOptions _restClientOptions;

    public GetPropertyApplicationRequestDetailQueryHandler(
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
        ILogger<GetPropertyApplicationRequestDetailQueryHandler> logger,
        IQueryRestClient queryRestClient,
        IOptions<RestClientOptions> restClientOptions)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
        _logger = logger;
        _queryRestClient = queryRestClient;
        _restClientOptions = restClientOptions.Value;
    }

    public async Task<PropertyApplicationRequestDetailDto> HandleAsync(QueryPropertyApplicationDetail query)
    {
        var propertyApplication = await _tenantPropertyApplicationRepository.GetByIdAsync(query.PropertyApplicationId);
        if (propertyApplication is null)
        {
            _logger.LogDebug("Property application not found.");
            return null!;
        }

        var httpResponse = await _queryRestClient.GetAsync<GetPropertyApplicationDetailDto>($"{_restClientOptions.Properties}/property/{propertyApplication.PropertyId}/detail/application-request/");
        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogDebug("Property Http request responding: {statusCode}", httpResponse.StatusCode);
            return null!;
        }

        var property = httpResponse.Content;

        var response = new PropertyApplicationRequestDetailDto()
        {
            PropertyApplicationId = propertyApplication.Id,
            TenantId = propertyApplication.ApplicantId,
            PropertyDetails = new()
            {
                Area = property.Area,
                Address = property.Address,
                AmountFt2 = property.AmountFt2,
                AmountBeds = property.AmountBeds,
                AmountBaths = property.AmountBaths,
                AmountParkingSpaces = property.AmountParkingSpaces,
                PropertyFronting = property.PropertyFronting,
                Pets = property.Pets,
                Image = property.Image,
            }
        };

        return response;
    }
}

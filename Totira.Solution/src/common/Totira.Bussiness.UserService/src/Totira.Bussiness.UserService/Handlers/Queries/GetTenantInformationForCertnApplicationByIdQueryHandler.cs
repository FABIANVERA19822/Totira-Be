using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetTenantInformationForCertnApplicationByIdQueryHandler : IQueryHandler<QueryTenantInformationForCertnApplicationById, GetTenantInformationForCertnApplicationDto>
{
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
    private readonly IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> _getTenantContactInformationByTenantId;
    private readonly ILogger<GetTenantInformationForCertnApplicationByIdQueryHandler> _logger;

    public GetTenantInformationForCertnApplicationByIdQueryHandler(
        IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
        IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto> getTenantContactInformationByTenantId,
        ILogger<GetTenantInformationForCertnApplicationByIdQueryHandler> logger)
    {
        _tenantBasicInformationRepository = tenantBasicInformationRepository;
        _getTenantContactInformationByTenantId = getTenantContactInformationByTenantId;
        _logger = logger;
    }

    public async Task<GetTenantInformationForCertnApplicationDto> HandleAsync(QueryTenantInformationForCertnApplicationById query)
    {
        var basicInformation = await _tenantBasicInformationRepository.GetByIdAsync(query.TenantId);
        var tenantId = query.TenantId;

        if (basicInformation is null)
        {
            _logger.LogWarning($"Basic information is missing for Tenant {tenantId}", query.TenantId);
            return default!;
        }
        if (basicInformation.SocialInsuranceNumber != null && basicInformation.SocialInsuranceNumber.Length != 9)
        {
            _logger.LogWarning($"Wrong Social Insurance Number, must be 9 characters or null for Tenant {tenantId}", tenantId);
            return default!;
        }


        var contactInformation = await _getTenantContactInformationByTenantId.HandleAsync(new QueryTenantContactInformationByTenantId(query.TenantId));

        if (contactInformation is null) 
        {
            _logger.LogWarning($"Contact Information is missing for Tenant {tenantId}", query.TenantId);
            return default!;
        }

        var response = new GetTenantInformationForCertnApplicationDto()
        {
            TenantId = query.TenantId,
            FirstName = basicInformation.FirstName,
            LastName = basicInformation.LastName,
            Birthday = GetFormattedBirthday(basicInformation.Birthday!),
            SocialInsuranceNumber = basicInformation.SocialInsuranceNumber!,
            PropertyLocation = new()
        };

        if (contactInformation is not null)
        {
            response.Addresses.Add(
                (new GetTenantInformationForCertnApplicationDto.AddressDto()
                {
                    Address = contactInformation.StreetAddress,
                    City = contactInformation.City,
                    Country = contactInformation.SelectedCountry,
                    State = contactInformation.Province,
                    Current = true
                }));
        }

        return response;
    }

    public static string GetFormattedBirthday(Birthday birthday)
        => new DateTime(birthday.Year, birthday.Month, birthday.Day).ToString("yyyy-MM-dd");
}

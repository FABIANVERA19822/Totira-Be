using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.GroupTenant.Certn;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Extensions.DomainExtensions;
using Totira.Bussiness.UserService.Queries.Group;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetGroupApplicantsInfoByTenantIdQueryHandler : IQueryHandler<QueryGetGroupApplicantsInfoByTenantId, GetGroupApplicantsInfoByTenantIdDto>
{
    private readonly ILogger<GetGroupApplicantsInfoByTenantIdQueryHandler> _logger;
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
    private readonly IRepository<TenantContactInformation, Guid> _tenantContactInformationRepository;
    private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;

    public GetGroupApplicantsInfoByTenantIdQueryHandler(
        ILogger<GetGroupApplicantsInfoByTenantIdQueryHandler> logger,
        IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
        IRepository<TenantContactInformation, Guid> tenantContactInformationRepository,
        IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository)
    {
        _logger = logger;
        _tenantBasicInformationRepository = tenantBasicInformationRepository;
        _tenantContactInformationRepository = tenantContactInformationRepository;
        _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
    }

    public async Task<GetGroupApplicantsInfoByTenantIdDto> HandleAsync(QueryGetGroupApplicantsInfoByTenantId query)
    {
        var response = new GetGroupApplicantsInfoByTenantIdDto()
        {
            Completed = GroupApplicantCompletedInfo.None.GetEnumDescription()
        };

        var applicationRequest = await _tenantApplicationRequestRepository.LastOrDefault(x => x.TenantId == query.TenantId);

        if (applicationRequest is null)
            return response;

        var completed = GroupApplicantCompletedInfo.All;

        // First main tenant
        var mainTenantInfo = new TenantApplicantDto()
        {
            IsMainTenant = true,
            Status = TenantApplicantInfoStatus.Complete.GetEnumDescription(),
        };

        await FillInformation(applicationRequest.TenantId, mainTenantInfo);

        if (mainTenantInfo.Status == TenantApplicantInfoStatus.MissingBasic.GetEnumDescription() ||
            mainTenantInfo.Status == TenantApplicantInfoStatus.MissingContact.GetEnumDescription())
        {
            return response;
        }

        response.Applicants.Add(mainTenantInfo);

        if (applicationRequest.Guarantor is not null)
        {
            // Now the guarantor
            var guarantorInfo = new TenantApplicantDto()
            {
                IsGuarantor = true,
                Name = applicationRequest.Guarantor.FirstName,
                Email = applicationRequest.Guarantor.Email,
            };

            if (applicationRequest.Guarantor.Id.HasValue)
            {
                guarantorInfo.Status = TenantApplicantInfoStatus.Complete.GetEnumDescription();

                await FillInformation(applicationRequest.Guarantor.Id.Value, guarantorInfo);
                
                if (guarantorInfo.Status == TenantApplicantInfoStatus.MissingBasic.GetEnumDescription() ||
                    guarantorInfo.Status == TenantApplicantInfoStatus.MissingContact.GetEnumDescription())
                {
                    return response;
                }
            }
            else
                completed = GroupApplicantCompletedInfo.Almost;
            
            response.Applicants.Add(guarantorInfo);
        }
        else
        {
            completed = GroupApplicantCompletedInfo.Almost;
        }
        
        // Now the co-applicants
        if (applicationRequest.Coapplicants is not null && applicationRequest.Coapplicants.Any())
        {
            foreach (var coapplicant in applicationRequest.Coapplicants)
            {
                var coapplicantInfo = new TenantApplicantDto()
                {
                    Name = coapplicant.FirstName,
                    Email = coapplicant.Email,
                };

                if (coapplicant.Id.HasValue)
                {
                    coapplicantInfo.Status = TenantApplicantInfoStatus.Complete.GetEnumDescription();
                    
                    await FillInformation(coapplicant.Id.Value, coapplicantInfo);

                    response.Applicants.Add(coapplicantInfo);
                }
                else
                {
                    completed = GroupApplicantCompletedInfo.Almost;
                }
            }
        }
        else
        {
            completed = GroupApplicantCompletedInfo.Almost;
        }

        response.Completed = completed.GetEnumDescription();

        return response;
    }

    /// <summary>
    /// Fills tenant information object by tenant ID.
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="dto">Data transfer object</param>
    private async Task FillInformation(Guid tenantId, TenantApplicantDto dto)
    {
        var response = new GetTenantInformationForCertnApplicationDto();
        var tenantBasicInformation = await _tenantBasicInformationRepository.GetByIdAsync(tenantId);
        
        dto.Name = $"{tenantBasicInformation.FirstName} {tenantBasicInformation.LastName}";
        dto.Email = tenantBasicInformation.TenantEmail;

        if (tenantBasicInformation is null || tenantBasicInformation.HasMissingInformation())
        {
            _logger.LogError("Tenant {tenantId} has missing basic information.", tenantId);
            dto.Status = TenantApplicantInfoStatus.MissingBasic.GetEnumDescription();
            dto.Information = response;
            return;
        }
        
        response.TenantId = tenantId;
        response.FirstName = tenantBasicInformation.FirstName;
        response.LastName = tenantBasicInformation.LastName;
        response.Birthday = GetFormattedBirthday(tenantBasicInformation.Birthday!);
        response.SocialInsuranceNumber = tenantBasicInformation.SocialInsuranceNumber ?? string.Empty;
        response.PropertyLocation = new();

        var tenantContactInformation = await _tenantContactInformationRepository.LastOrDefault(x => x.TenantId == tenantId);
        if (tenantContactInformation is null || tenantContactInformation.HasMissingInformation())
        {
            _logger.LogError("Tenant {tenantId} has missing contact information.", tenantId);
            dto.Status = TenantApplicantInfoStatus.MissingContact.GetEnumDescription();
            dto.Information = response;
            return;
        }

        response.Addresses.Add(new()
        {
            Address = tenantContactInformation.StreetAddress,
            City = tenantContactInformation.City,
            Country = tenantContactInformation.Country,
            State = tenantContactInformation.Province,
            Current = true
        });
        dto.Information = response;

        return;
    }

    public static string GetFormattedBirthday(TenantBasicInformation.TenantBirthday birthday)
        => new DateTime(birthday.Year, birthday.Month, birthday.Day).ToString("yyyy-MM-dd");
}

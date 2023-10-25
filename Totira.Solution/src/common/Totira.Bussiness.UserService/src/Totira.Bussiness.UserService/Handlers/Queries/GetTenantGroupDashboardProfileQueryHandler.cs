using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.GroupTenant;
using Totira.Bussiness.UserService.Extensions.DomainExtensions;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Queries.Group;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetTenantGroupDashboardProfileQueryHandler : IQueryHandler<QueryGetGroupDashboardProfileByTenantId, GetTenantGroupDashboardProfileDto>
{
    private readonly IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> _getTenantVerifiedProfileByIdQueryHandler;
    private readonly IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> _getTenantEmployeeIncomesById;
    private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
    private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
    private readonly ICommonFunctions _commonFunctions;

    public GetTenantGroupDashboardProfileQueryHandler(
        IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> getTenantVerifiedProfileByIdQueryHandler,
        IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> getTenantEmployeeIncomesById,
        IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
        IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
        ICommonFunctions commonFunctions,
        IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository)
    {
        _getTenantVerifiedProfileByIdQueryHandler = getTenantVerifiedProfileByIdQueryHandler;
        _getTenantEmployeeIncomesById = getTenantEmployeeIncomesById;
        _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
        _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
        _commonFunctions = commonFunctions;
        _tenantBasicInformationRepository = tenantBasicInformationRepository;
    }

    public async Task<GetTenantGroupDashboardProfileDto> HandleAsync(QueryGetGroupDashboardProfileByTenantId query)
    {
        var tenantApplicationRequest = await _tenantApplicationRequestRepository.LastOrDefault(x => x.TenantId == query.TenantId);
        if (tenantApplicationRequest is null)
            return default!;
        
        var tenantApplicationDetails = await _tenantApplicationDetailsRepository.LastOrDefault(x => x.TenantId == query.TenantId);
        if (tenantApplicationDetails is null)
            return default!;

        var response = new GetTenantGroupDashboardProfileDto(query.TenantId, tenantApplicationRequest.Id);
        response.ApplicationDetails = new(totalIncomes: 0,
            GetEstimatedMove(tenantApplicationDetails),
            tenantApplicationDetails.EstimatedRent,
            new(tenantApplicationDetails.Occupants.Adults, tenantApplicationDetails.Occupants.Childrens),
            HavePets(tenantApplicationDetails),
            GetPets(tenantApplicationDetails)?.ToList(),
            HaveCars(tenantApplicationDetails),
            GetCars(tenantApplicationDetails!)?.ToList());

        var coapplicants = new List<CoApplicantDto>();
        coapplicants.Add(new(tenantApplicationRequest.TenantId, true, false, string.Empty));

        if (tenantApplicationRequest.Coapplicants is not null && tenantApplicationRequest.Coapplicants.Any())
            coapplicants.AddRange(tenantApplicationRequest.Coapplicants.Select(x => new CoApplicantDto(x.Id, false, false, x.FirstName)));

        if (tenantApplicationRequest.Guarantor is not null && tenantApplicationRequest.Guarantor.Id is not null)
            coapplicants.Add(new(tenantApplicationRequest.Guarantor.Id, false, true, tenantApplicationRequest.Guarantor.FirstName));

        foreach (var coapplicant in coapplicants)
        {
            if (coapplicant.TenantId.HasValue)
            {
                var incomesQuery = new QueryTenantEmployeeIncomesById(coapplicant.TenantId.Value);
                var incomes = await _getTenantEmployeeIncomesById.HandleAsync(incomesQuery);

                var verifiedProfileQuery = new QueryTenantVerifiedProfileById(coapplicant.TenantId.Value, incomes);
                var verifiedProfile = await _getTenantVerifiedProfileByIdQueryHandler.HandleAsync(verifiedProfileQuery);

                var profileImage = await _commonFunctions.GetProfilePhoto(new(coapplicant.TenantId.Value));
                
                var coapplicantBasicInformation = await _tenantBasicInformationRepository.GetByIdAsync(coapplicant.TenantId.Value);
                var coapplicantName = $"{coapplicantBasicInformation.FirstName} {coapplicantBasicInformation.LastName}";
                
                response.ApplicationDetails.TotalIncomes += verifiedProfile.EmployeeIncome.Employee.Sum(x => x.MonthlySalary);
                response.Tenants.Add(new(profileImage, coapplicant.IsMain, coapplicant.IsGuarantor, verifiedProfile, coapplicantName));
            }
            else
            {
                response.Tenants.Add(new(null, coapplicant.IsMain, coapplicant.IsGuarantor, null, coapplicant.Name));
            }
        }

        return response;
    }

    private static bool HavePets(TenantApplicationDetails entity) => entity.Pets is not null && entity.Pets.Any();
    private static IEnumerable<ApplicationDetailsPetDto>? GetPets(TenantApplicationDetails entity) => HavePets(entity)
        ? entity.Pets!.Select(x => new ApplicationDetailsPetDto(x.Type, x.Size, x.Description))
        : null;
    private static bool HaveCars(TenantApplicationDetails entity) => entity.Cars is not null && entity.Cars.Any();
    private static IEnumerable<ApplicationDetailsCarDto>? GetCars(TenantApplicationDetails entity) => HaveCars(entity)
        ? entity.Cars!.Select(x => new ApplicationDetailsCarDto(x.Model, x.Make, x.Plate, x.Year))
        : null;

    private static ApplicationDetailsDateDto? GetEstimatedMove(TenantApplicationDetails entity) => entity.EstimatedMove is not null
        ? new(entity.EstimatedMove.Month, entity.EstimatedMove.Year)
        : null;

    private class CoApplicantDto
    {
        public CoApplicantDto(Guid? tenantId, bool isMain, bool isGuarantor, string name)
        {
            TenantId = tenantId;
            IsMain = isMain;
            IsGuarantor = isGuarantor;
            Name = name;
        }

        public string Name { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsMain { get; set; }
        public bool IsGuarantor { get; set; }
    }
}
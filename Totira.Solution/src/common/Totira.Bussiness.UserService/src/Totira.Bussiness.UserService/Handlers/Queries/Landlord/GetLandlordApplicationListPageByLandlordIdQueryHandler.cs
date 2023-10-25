using Amazon.SimpleEmail.Model.Internal.MarshallTransformations;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Bussiness.UserService.DTO.PropertyService;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Queries.Landlord;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using Totira.Support.Persistance.Mongo.Util;
using Totira.Support.Persistance.Util;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries.Landlord
{
    public class GetLandlordApplicationListPageByLandlordIdQueryHandler : IQueryHandler<QueryLandlordApplicationsListPageByLandlordId, GetApplicationListPageDto>
    {
        private readonly ILogger<GetLandlordApplicationListPageByLandlordIdQueryHandler> _logger;
        private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
        private readonly IRepository<TenantApplicationDetails, Guid> _tenatApplicationDetailsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantbasicInformationRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _applicationRequestRepository;

        private readonly IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> _getTenantVerifiedProfileByIdQueryHandler;
        private readonly IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> _getTenantEmployeeIncomesById;
        private readonly ICommonFunctions _commonFunctions;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;


        public GetLandlordApplicationListPageByLandlordIdQueryHandler(
            IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
            ILogger<GetLandlordApplicationListPageByLandlordIdQueryHandler> logger,
            IRepository<TenantApplicationDetails, Guid> tenatApplicationDetailsRepository,
            IRepository<TenantBasicInformation, Guid> tenantbasicInformationRepository,
            IRepository<TenantApplicationRequest, Guid> applicationRequestRepository,
            IQueryHandler<QueryTenantVerifiedProfileById, GetTenantVerifiedbyProfileDto> getTenantVerifiedProfileByIdQueryHandler,
            IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto> getTenantEmployeeIncomesById,
            ICommonFunctions commonFunctions,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions
            )
        {
            _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
            _logger = logger;
            _commonFunctions = commonFunctions;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _tenatApplicationDetailsRepository = tenatApplicationDetailsRepository;
            _tenantbasicInformationRepository = tenantbasicInformationRepository;
            _applicationRequestRepository = applicationRequestRepository;
            _getTenantEmployeeIncomesById = getTenantEmployeeIncomesById;
            _getTenantVerifiedProfileByIdQueryHandler = getTenantVerifiedProfileByIdQueryHandler;

        }

        public async Task<GetApplicationListPageDto> HandleAsync(QueryLandlordApplicationsListPageByLandlordId query)
        {
            _logger.LogInformation($"validate existence of property applications relate to PropertyId {query.PropertyId}");
            var result = new GetApplicationListPageDto();

            IMongoFilter<TenantPropertyApplication> filter = new MongoFilter<TenantPropertyApplication>();

            if (!string.IsNullOrWhiteSpace(query.PropertyId))
                filter.AddCondition(propertyApplication => propertyApplication.PropertyId.Equals(query.PropertyId));

            var propertyApplicationList = await _tenantPropertyApplicationRepository.GetPageAsync(filter, query.PageNumber, query.PageSize, "CreatedOn", descending: true);

            result.SortBy = query.SortBy;
            result.PageNumber = query.PageNumber;
            result.PageSize = query.PageSize;
            result.Count = await _tenantPropertyApplicationRepository.GetCountAsync(filter);

            if (!propertyApplicationList.Any())
            {
                _logger.LogInformation("PropertyId doesnt match with any data");
                return new GetApplicationListPageDto();
            }

            Expression<Func<TenantPropertyApplication, bool>> expression = p => p.PropertyId == query.PropertyId && p.Status.Equals(TenantPropertyApplicationStatusEnum.Approved.GetEnumDescription());
            var propertyApplicationStatus = (await _tenantPropertyApplicationRepository.Get(expression)).FirstOrDefault();
        
            // data of property widget

            var propertyData = await _queryRestClient.GetAsync<GetPropertyDetailstoApplyDto>($"{_restClientOptions.Properties}/Property/propertyDetails/{query.PropertyId}");
            var propertyImageFile = new PropertyMainImage(propertyData.Content.PropertyImageFile.FileName, propertyData.Content.PropertyImageFile.ContentType, propertyData.Content.PropertyImageFile.FileUrl);
            result.PropertyApplied = new PropertyApplied(propertyData.Content.Id, propertyImageFile, propertyData.Content.Area, propertyData.Content.Address, propertyData.Content.ListPrice, propertyApplicationStatus is not null && propertyApplicationStatus.Status.Equals(TenantPropertyApplicationStatusEnum.Approved.GetEnumDescription()));

            // data of applicationList
            foreach (var propertyapplication in propertyApplicationList)
            {
                _logger.LogInformation($"Obtain applicationDetailfromRequestData and basicInfo from  applicationRequestId: {propertyapplication.ApplicationRequestId} and MainTenantId {propertyapplication.MainTenantId}");

                var applicationDetailfromRequest = (await _applicationRequestRepository.GetByIdAsync(propertyapplication.ApplicationRequestId)).ApplicationDetailsId;
                var applicationDetailsData = await _tenatApplicationDetailsRepository.GetByIdAsync(applicationDetailfromRequest!.Value);
                var tenantBasicInfoData = await _tenantbasicInformationRepository.GetByIdAsync(propertyapplication.MainTenantId);
                var profilePhoto = await _commonFunctions.GetProfilePhoto(new QueryTenantProfileImageById(propertyapplication.MainTenantId));
                var CosignersData = await GetDataCosigners(propertyapplication.CoApplicantsIds, propertyData.Content.ListPrice);

                _logger.LogInformation($"Obtain backgroundTenant by MainTenantId {propertyapplication.MainTenantId}, comparison propertyListPrice {propertyData.Content.ListPrice}");

                var backgroundTenant = await GetBackgroundbytenantId(propertyapplication.MainTenantId, propertyData.Content.ListPrice);
                    backgroundTenant.ApplicationRequest = propertyapplication.ApplicationRequestId;
                
                var totalIncomes = GetTotalIncomes(CosignersData, backgroundTenant);

                var applicationDashboard = new ApplicationDashboard()
                {
                    StatusApplication = propertyapplication.Status,
                    PropertyApplicationId = propertyapplication.Id,
                    SubmissionDate = propertyapplication.CreatedOn,
                    Applicant = new Applicant()
                    {
                        NumberCosigners = propertyapplication.IsMulti ? propertyapplication.CoApplicantsIds.Count() : 0,
                        MainApplicantName = tenantBasicInfoData.FirstName,
                        isMulti = propertyapplication.IsMulti,
                        Cosigners = propertyapplication.IsMulti ? CosignersData : null,
                        MainApplicantProfileImage = profilePhoto,
                        BackgroundMainTenantDetail = backgroundTenant

                    },
                    ApplicationDetails = new ApplicationDetailsDto()
                    {
                        ApplicationDetailsId = applicationDetailsData.Id,
                        EstimatedRent = applicationDetailsData.EstimatedRent,
                        EstimatedMove = GetEstimatedMove(applicationDetailsData),
                        TotalIncome = totalIncomes,
                        Occupants = new DTO.Landlord.ApplicationDetailOccupants(applicationDetailsData.Occupants.Adults, applicationDetailsData.Occupants.Childrens)
                    }
                };
                result.Applications.Add(applicationDashboard);
            }

            _logger.LogInformation($"ListSorting of Dashboard by querySortBy: {query.SortBy.GetEnumDescription()}");

            result.Applications = SortingDashboard(result.Applications, query.SortBy, query.Asc);

            return result;
        }

        private async Task<GetApplicationListPageDetails> GetBackgroundbytenantId(Guid tenantId, decimal propertyprice)
        {
            _logger.LogInformation($"Start GetBackgroundbytenantId task - Obtain incomes from _getTenantEmployeeIncomesById Handler of tenantId {tenantId}");

            var background = new GetApplicationListPageDetails();
            var incomesQuery = new QueryTenantEmployeeIncomesById(tenantId);
            var incomes = await _getTenantEmployeeIncomesById.HandleAsync(incomesQuery);

            _logger.LogInformation($"Obtain verifiedProfileData from getTenantVerifiedProfileByIdQueryHandler of tenantId {tenantId} with incomes: {incomes}");
            var verifiedProfileQuery = new QueryTenantVerifiedProfileById(tenantId, incomes);
            var verifiedProfile = await _getTenantVerifiedProfileByIdQueryHandler.HandleAsync(verifiedProfileQuery);        
            var TotalIncome = verifiedProfile.EmployeeIncome.Employee.Any() ? verifiedProfile.EmployeeIncome.Employee.Sum(x => x.MonthlySalary) : 0;
            
            background.TotalIncome = TotalIncome;
            background.TypeIncome = verifiedProfile.EmployeeIncome.Student.Any()? "Student" : "Income";

            background.BackgroundCheckStatus = GetBackgroundUnified(verifiedProfile.BackgroundCheck);
            background.CreditScore.Status = verifiedProfile.CreditRating.Status;
            background.CreditScore.Score = verifiedProfile.CreditRating.Score;
            background.PropertyPriceIncomeScale = TotalIncome > 0 ? (int)Math.Round(TotalIncome / propertyprice): 0;

            _logger.LogInformation($"Terminate GetBackgroundbytenantId Process by tenantId {tenantId}");
            return background;
        }
        private string GetBackgroundUnified(BackgroundCheck backgroundCheck)
        {
            if (backgroundCheck.FraudStatus.Equals("CLEARED") && 
                backgroundCheck.CourtRecordsStatus.Equals("CLEARED") && 
                backgroundCheck.CriminalConvictionStatus.Equals("CLEARED") && 
                backgroundCheck.SexOffenderStatus.Equals("CLEARED"))
            {
                return "CLEAR";
            }
            else
                return "REVIEW"; 
        }
        private async Task<List<Cosigners>> GetDataCosigners(List<Guid> coApplicantsIds, decimal propertyprice)
        {
            _logger.LogInformation($"Start GetDataCosigners task");

            var result = new List<Cosigners>();
            foreach (var co in coApplicantsIds)
            {
                var profilePhoto = await _commonFunctions.GetProfilePhoto(new QueryTenantProfileImageById(co));
                var tenantBasicInfo = await _tenantbasicInformationRepository.GetByIdAsync(co);
                var backgroundCosignerDetail = await GetBackgroundbytenantId(co, propertyprice);

                result.Add(new Cosigners()
                {
                    CosignersProfileImage = profilePhoto,
                    CosignerName = tenantBasicInfo.FirstName,
                    BackgroundCosignerDetail= backgroundCosignerDetail 
                });
            }
            _logger.LogInformation($"Terminate GetDataCosigners Process");
            return result;
        }
        private static double GetTotalIncomes(List<Cosigners> cosignersData, GetApplicationListPageDetails backgroundtenant)
        {
            var TotalIncomes = backgroundtenant.TotalIncome;
            foreach (var co in cosignersData)
            {
                TotalIncomes += co.BackgroundCosignerDetail.TotalIncome;
            }
            return TotalIncomes;
        }
        private static List<ApplicationDashboard> SortingDashboard(IEnumerable<ApplicationDashboard> applications, SortApplicationsBy sortBy, bool isAsc)
        {
            switch (sortBy,isAsc)
            {
                case (SortApplicationsBy.SubmissionDate, true):
                    applications = applications.OrderBy(s => s.SubmissionDate);
                    break;
               
                case (SortApplicationsBy.Applicant, true):
                    applications = applications.OrderBy(s => s.Applicant.MainApplicantName);
                    break;

                case (SortApplicationsBy.TotalIncome, true):
                    applications = applications.OrderBy(s => s.ApplicationDetails.TotalIncome);
                    break;

                case (SortApplicationsBy.MoveInDate, true):
                    applications = applications.OrderBy(s => new DateTime(s.ApplicationDetails.EstimatedMove.Year,s.ApplicationDetails.EstimatedMove.Month,1));
                    break;

                case (SortApplicationsBy.Duration, true):
                    applications = applications.OrderBy(s => s.ApplicationDetails.EstimatedRent);
                    break;

                case (SortApplicationsBy.Occupants, true):
                    applications = applications.OrderBy(s => s.ApplicationDetails.Occupants.Adults + s.ApplicationDetails.Occupants.Children);
                    break;


                case (SortApplicationsBy.SubmissionDate, false):
                    applications = applications.OrderByDescending(s => s.SubmissionDate);
                    break;
                case (SortApplicationsBy.Applicant, false):
                    applications = applications.OrderByDescending(s => s.Applicant.MainApplicantName);
                    break;
                case (SortApplicationsBy.TotalIncome, false):
                    applications = applications.OrderByDescending(s => s.ApplicationDetails.TotalIncome);
                    break;
                case (SortApplicationsBy.MoveInDate, false):
                    applications = applications.OrderByDescending(s => new DateTime(s.ApplicationDetails.EstimatedMove.Year, s.ApplicationDetails.EstimatedMove.Month, 1));
                    break;
                case (SortApplicationsBy.Duration, false):
                    applications = applications.OrderByDescending(s => s.ApplicationDetails.EstimatedRent);
                    break;
                case (SortApplicationsBy.Occupants, false):
                    applications = applications.OrderByDescending(s => s.ApplicationDetails.Occupants.Adults + s.ApplicationDetails.Occupants.Children);
                    break;

                default:
                    applications = applications.OrderByDescending(s => s.SubmissionDate);
                    break;
            }
            return applications.ToList();
        }
        private static DTO.ApplicationDetailEstimatedMove? GetEstimatedMove(TenantApplicationDetails entity) => entity.EstimatedMove is not null
        ? new(entity.EstimatedMove.Month, entity.EstimatedMove.Year)
        : null;

        

    }
}

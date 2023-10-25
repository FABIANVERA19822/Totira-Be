using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Bussiness.UserService.DTO.PropertyService;

using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;
using System.Text;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
	public class GetTenantApplicationListPageByIdQueryHandler : IQueryHandler<QueryTenantApplicationListPageById, GetTenantApplicationListPageDto>
    {
        private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> _getTenantProfileImageByIdHandler;
        private readonly IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> _getTenantPersonalInfoByIdHandler;
        private readonly IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> _getTenantApllicationDetailsByIdHandler;
        public GetTenantApplicationListPageByIdQueryHandler(
            IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
                        IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> getTenantProfileImageByIdHandler,
            IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> getTenantPersonalInfoByIdHandler,
            IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto> getTenantApllicationDetailsByIdHandler
            )
		{
            _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _getTenantProfileImageByIdHandler = getTenantProfileImageByIdHandler;
            _getTenantPersonalInfoByIdHandler = getTenantPersonalInfoByIdHandler;
            _getTenantApllicationDetailsByIdHandler = getTenantApllicationDetailsByIdHandler;
        }

        public async Task<GetTenantApplicationListPageDto> HandleAsync(QueryTenantApplicationListPageById query)
        {
            var propertyApplications = (await _tenantPropertyApplicationRepository.Get(p => p.ApplicantId == query.Id || p.MainTenantId == query.Id || p.CoApplicantsIds.Any(i => i== query.Id))).ToList();

            var result = new GetTenantApplicationListPageDto();

            if (propertyApplications.Any())
            {
                foreach (var item in propertyApplications)
                {
                   var oneApplicationDto = new TenantSingleApplication();
                    oneApplicationDto.PropertyId = item.PropertyId;
                    oneApplicationDto.Status = item.Status;
                    oneApplicationDto.CreatedOn = item.CreatedOn;

                    var propertyData = await _queryRestClient.GetAsync<GetPropertyDetailstoApplyDto>($"{_restClientOptions.Properties}/Property/propertyDetails/{item.PropertyId}");
                    oneApplicationDto.PropertyImageFile = new PropertyMainImage(propertyData.Content.PropertyImageFile.FileName, propertyData.Content.PropertyImageFile.ContentType, propertyData.Content.PropertyImageFile.FileUrl);
                    oneApplicationDto.Area = propertyData.Content.Area;
                    oneApplicationDto.Address = propertyData.Content.Address;
                    oneApplicationDto.ListPrice = propertyData.Content.ListPrice;

                    
                    if (item.IsMulti)
                    {
                        oneApplicationDto.ApplicationDetails = await _getTenantApllicationDetailsByIdHandler.HandleAsync(new QueryTenantApplicationDetailsById(item.ApplicantId));
                        var applicantInfo = await GetApplicantInfo(item.ApplicantId, "Single Applicant");
                        
                        oneApplicationDto.CoApplicantsInfo.Add(applicantInfo);
                        
                    }
                    else
                    {
                        oneApplicationDto.ApplicationDetails = await _getTenantApllicationDetailsByIdHandler.HandleAsync(new QueryTenantApplicationDetailsById(item.MainTenantId));

                        var mainApplicantInfo = await GetApplicantInfo(item.MainTenantId, "Main Applicant");
                        
                        oneApplicationDto.CoApplicantsInfo.Add(mainApplicantInfo);

                        foreach (var applicantId in item.CoApplicantsIds)
                        {
                            var applicantInfo = await GetApplicantInfo(applicantId, "CoApplicant");
                            
                            oneApplicationDto.CoApplicantsInfo.Add(applicantInfo);
                        }
                    }

                    result.ApplicationListPage.Add(oneApplicationDto);
                } 

                if (query.PageNumber <= 0)
                    query.PageNumber = 1;

                result.ApplicationListPage = result.ApplicationListPage.OrderByDescending(p => p.CreatedOn).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

            }

            return result;
        }
        private async Task<CoApplicantsImageAndName> GetApplicantInfo(Guid id, string role)
        {
            var ProfileImageDto = await _getTenantProfileImageByIdHandler.HandleAsync(new QueryTenantProfileImageById(id));
            var GetTenantBasicInformationDto = await _getTenantPersonalInfoByIdHandler.HandleAsync(new QueryTenantBasicInformationById(id));

            StringBuilder TenantName = new StringBuilder();
            TenantName.Append(GetTenantBasicInformationDto.FirstName);
            TenantName.Append($" {GetTenantBasicInformationDto.LastName}");


            var result = new CoApplicantsImageAndName
            {

                CoApplicantsImage = ProfileImageDto,
                Role = role,
                CoApplicantName = TenantName.ToString()
                

            };

            return result;
        }
    }
}


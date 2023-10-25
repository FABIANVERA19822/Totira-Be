using System;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
	public class GetTenantGroupApplicationSummaryByIdQueryHandler : IQueryHandler<QueryTenantGroupApplicationSummaryById, GetTenantGroupApplicationSummaryDto>
    {
        private readonly IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> _getTenantProfileImageByIdHandler;
        private readonly IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>> _getTenantProfileProgress;
        private readonly IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> _getTenantPersonalInfoByIdHandler;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;

        public GetTenantGroupApplicationSummaryByIdQueryHandler(IQueryHandler<QueryTenantProfileImageById, GetTenantProfileImageDto> getTenantProfileImageByIdHandler,
            IQueryHandler<QueryTenantProfileProgressByTenantId, Dictionary<string, int>> getTenantProfileProgress,
            IQueryHandler<QueryTenantBasicInformationById, GetTenantBasicInformationDto> getTenantPersonalInfoByIdHandler,
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository
            )
		{
            _getTenantProfileImageByIdHandler = getTenantProfileImageByIdHandler;
            _getTenantProfileProgress = getTenantProfileProgress;
            _getTenantPersonalInfoByIdHandler = getTenantPersonalInfoByIdHandler;
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
        }

        public async Task<GetTenantGroupApplicationSummaryDto> HandleAsync(QueryTenantGroupApplicationSummaryById query)
        {
            

          //  Expression<Func<TenantApplicationRequest, bool>> expression = (ap => ap.TenantId == query.Id || ap.Guarantor.Id == query.Id || ap.Coapplicants.Any(i=>i.Id == query.Id));
            var applicationsRequest = (await _tenantApplicationRequestRepository.Get(ap => ap.TenantId == query.Id || ap.Guarantor.Id == query.Id || ap.Coapplicants.Any(i => i.Id == query.Id))).FirstOrDefault();
            if (applicationsRequest == null)
            {

                var Result = new GetTenantGroupApplicationSummaryDto
                {
                    MainApplicant = null,
                    Guarantor = null,
                    CoApplicants = null
                };
                return Result;
            }
            
            


          var mainApplicant = await GetApplicantInfo(applicationsRequest.TenantId);

            GetTenantIndividualApplicationSummaryDto guarantor = new GetTenantIndividualApplicationSummaryDto();
            if (applicationsRequest?.Guarantor?.Id != null)
            {
                 guarantor = await GetApplicantInfo(applicationsRequest.Guarantor.Id.Value);

            }

            List<GetTenantIndividualApplicationSummaryDto> coApplicants = new List<GetTenantIndividualApplicationSummaryDto>();
            if (applicationsRequest?.Coapplicants != null)
            {
                foreach (var item in applicationsRequest.Coapplicants)
                {
                    if (item?.Id != null)
                    {
                        var coApplicant = await GetApplicantInfo(item.Id.Value);
                        coApplicants.Add(coApplicant);
                    }
    
                }
            }

            var result = new GetTenantGroupApplicationSummaryDto
            {
                ApplicationRequestId = applicationsRequest?.Id,
                MainApplicant = mainApplicant,
                Guarantor = guarantor,
                CoApplicants = coApplicants
            };
            return result;
        }

        private async Task<GetTenantIndividualApplicationSummaryDto> GetApplicantInfo(Guid Id)
        {
           var ProfileImageDto = await _getTenantProfileImageByIdHandler.HandleAsync(new QueryTenantProfileImageById(Id));
           var ProfileProgressDto = await _getTenantProfileProgress.HandleAsync(new QueryTenantProfileProgressByTenantId(Id));
           var GetTenantBasicInformationDto = await _getTenantPersonalInfoByIdHandler.HandleAsync(new QueryTenantBasicInformationById(Id));

            StringBuilder TenantName = new StringBuilder();
            TenantName.Append(GetTenantBasicInformationDto.FirstName);
            TenantName.Append($" {GetTenantBasicInformationDto.LastName}");


            var result = new GetTenantIndividualApplicationSummaryDto
            {
                ID = Id,
                ProfileImage = ProfileImageDto,
                ProfileProgress = ProfileProgressDto,
                Name = TenantName.ToString()

            };

            return result;
        }

    }
}


using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static System.Net.Mime.MediaTypeNames;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{

    public class GetApplicationRequestByTenantIdQueryHandler : IQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto>
    {
        private readonly ILogger<CreateTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;

        public GetApplicationRequestByTenantIdQueryHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<CreateTenantApplicationRequestCommandHandler> logger
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
        }       

        public async Task<GetTenantApplicationRequestDto> HandleAsync(QueryApplicationRequestByTenantId query)
        {

            TenantApplicationRequest applicationRequest = null;
            if (query.ApplicationId != null)
            {
                applicationRequest = await _tenantApplicationRequestRepository.GetByIdAsync(query.ApplicationId.Value);
            }
            else
            {
                Expression<Func<TenantApplicationRequest, bool>> expression = (ap => ap.TenantId == query.TenantId
                                                                                  || ap.Coapplicants.Any(x=>x.Id==query.TenantId)
                                                                                  || ap.Guarantor.Id == query.TenantId);
                var applicationsRequest = await _tenantApplicationRequestRepository.Get(expression);
                if (applicationsRequest == null)
                {
                    _logger.LogError($"Current Tenant dont have any application request ");
                    return null;
                }

                applicationRequest = applicationsRequest.OrderByDescending(ap => ap.CreatedOn).FirstOrDefault();
            }

            if (applicationRequest == null)
            {
                return new GetTenantApplicationRequestDto();
            }

            var result = new GetTenantApplicationRequestDto()
            {
                Owner = applicationRequest.TenantId == query.TenantId,
                TenantId = applicationRequest.TenantId,
                ApplicationId = applicationRequest.Id,
                ApplicationDetailsId = applicationRequest.ApplicationDetailsId,
                IsMulti = applicationRequest.Guarantor != null ? true : applicationRequest.Coapplicants != null && applicationRequest.Coapplicants.Count() > 0,
                CreatedOn = applicationRequest.CreatedOn,
                Coapplicants = applicationRequest.Coapplicants != null ? applicationRequest.Coapplicants.Select(co => new CoApplicantDto { CoapplicantId = co.Id, Email = co.Email, FirstName= co.FirstName, Status = co.Status }).ToList() : null,
                Guarantor = applicationRequest.Guarantor != null ? new CoApplicantDto { CoapplicantId = applicationRequest.Guarantor.Id, Email = applicationRequest.Guarantor.Email, FirstName = applicationRequest.Guarantor.FirstName, Status = applicationRequest.Guarantor.Status }: null,
                //todo Get if useer are in validation process
                InValidationProcess = false
            };
            return result;
        }
    }
}

using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetAllApplicationRequestByTenantIdQueryHandler : IQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto>
    {   private readonly ILogger<GetAllApplicationRequestByTenantIdQueryHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;

        public GetAllApplicationRequestByTenantIdQueryHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,            
            ILogger<GetAllApplicationRequestByTenantIdQueryHandler> logger
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
        }

        public async Task<GetAllTenantApplicationRequestDto> HandleAsync(QueryAllApplicationRequestByTenantId query)
        {
            Expression<Func<TenantApplicationRequest, bool>> expression = (ap => ap.TenantId == query.TenantId);
            var applicationsRequest = await _tenantApplicationRequestRepository.Get(expression);
            if (applicationsRequest == null)
            {
                _logger.LogError($"Current Tenant dont have any application request ");
                return null;
            }

            var applicationRequest = applicationsRequest.OrderByDescending(ap => ap.CreatedOn);


            var applications = new List<GetTenantApplicationRequestDto>();

            if (applicationRequest.Any())
            {
                foreach (var application in applicationsRequest)
                {
                    var mapped = new GetTenantApplicationRequestDto()
                    {

                        Owner = application.TenantId == query.TenantId,
                        TenantId = application.TenantId,
                        ApplicationId = application.Id,
                        ApplicationDetailsId = application.ApplicationDetailsId,
                        IsMulti = application.Guarantor != null ? true : (application.Coapplicants != null && application.Coapplicants.Count() > 0),
                        CreatedOn = application.CreatedOn,
                        Coapplicants = application.Coapplicants != null ? application.Coapplicants.Select(co => new CoApplicantDto { CoapplicantId = co.Id, Email = co.Email,FirstName=co.FirstName, Status = co.Status }).ToList() : null,
                        Guarantor = application.Guarantor != null ? new CoApplicantDto { CoapplicantId = application.Guarantor.Id, Email = application.Guarantor.Email, FirstName= application.Guarantor.FirstName, Status = application.Guarantor.Status } : null,
                        //todo Get if user are in validation process
                        InValidationProcess = false
                    };

                    applications.Add(mapped);
                }

            }

            var result = new GetAllTenantApplicationRequestDto()
            {
                TenantId = query.TenantId,
                Applications = applications,
                CurrentActive = applications.Any() ? applications.MaxBy(a => a.CreatedOn).ApplicationId : null
            };            
           
            return result;
        }
    }
}

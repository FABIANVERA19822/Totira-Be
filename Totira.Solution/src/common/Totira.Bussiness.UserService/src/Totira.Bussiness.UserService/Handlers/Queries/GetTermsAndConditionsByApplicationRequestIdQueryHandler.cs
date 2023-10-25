using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTermsAndConditionsByApplicationRequestIdQueryHandler : IQueryHandler<QueryTermsAndConditionsByApplicationRequestId, bool?>
    {

        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> _termsAndConditionsRepository;
        private readonly ILogger<GetTermsAndConditionsByApplicationRequestIdQueryHandler> _logger;
        public GetTermsAndConditionsByApplicationRequestIdQueryHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> termsAndConditionsRepository,
            ILogger<GetTermsAndConditionsByApplicationRequestIdQueryHandler> logger)
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _termsAndConditionsRepository = termsAndConditionsRepository;
            _logger = logger;
        }
        public async Task<bool?> HandleAsync(QueryTermsAndConditionsByApplicationRequestId query)
        {

            bool result = true;

            var request = (await _tenantApplicationRequestRepository.GetByIdAsync(query.ApplicationRequestId));

            if (request is null)
            {
                _logger.LogError("Request {applicationRequestId} does not exist.", query.ApplicationRequestId);
                return null;
            }

            result = (await _termsAndConditionsRepository.Get(x => x.TenantId == request.TenantId)).Any();

            if (request.Coapplicants is not null)
            {
                foreach (var coapplicant in request.Coapplicants)
                {
                    if (coapplicant.Id.HasValue)
                    {
                        result = result && (await _termsAndConditionsRepository.Get(x => x.TenantId == coapplicant.Id)).Any();
                    }
                }
            }


            if (request.Guarantor is not null && request.Guarantor.Id.HasValue)
            {
                result = result && (await _termsAndConditionsRepository.Get(x => x.TenantId == request.Guarantor.Id)).Any();
            }

            return result;
        }
    }
}

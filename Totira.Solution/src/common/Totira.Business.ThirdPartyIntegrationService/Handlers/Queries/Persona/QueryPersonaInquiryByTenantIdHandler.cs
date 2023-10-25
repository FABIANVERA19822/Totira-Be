using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Persona;
using Totira.Business.ThirdPartyIntegrationService.Queries.Persona;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Persona
{
    public class QueryPersonaInquiryByTenantIdHandler : IQueryHandler<QueryPersonaInquiryByTenantId, GetPersonaApplicationDto>
    {

        private readonly IRepository<TenantPersonaValidation, string> _tenantPersonalValidationRepository;
        private readonly ILogger<TenantPersonaValidationCommandHandler> _logger;

        public QueryPersonaInquiryByTenantIdHandler(
            IRepository<TenantPersonaValidation, string> tenantPersonalValidationRepository,
            ILogger<TenantPersonaValidationCommandHandler> logger)
        {

            _tenantPersonalValidationRepository = tenantPersonalValidationRepository;
            _logger = logger;
        }

        public async Task<GetPersonaApplicationDto> HandleAsync(QueryPersonaInquiryByTenantId query)
        {
            Expression<Func<TenantPersonaValidation, bool>> expression = (inq => inq.TenantId == query.TenantId);
            var info = await _tenantPersonalValidationRepository.Get(expression);

            if (!info.Any()) {
                _logger.LogError($"tenant with id {query.TenantId} dont have any inquiry with persona");
                return new GetPersonaApplicationDto();
            }

            var latest = info.MaxBy(i => i.CreatedOn);

            return new GetPersonaApplicationDto() { InquiryId = latest.Id, Status = latest.Status, TenantId = latest.TenantId, UrlImages = latest.UrlImages };

            
        }
    }
}

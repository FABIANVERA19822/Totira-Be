using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Persona;
using Totira.Business.ThirdPartyIntegrationService.Queries.Jira;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Jira
{
    public class QueryJiraTicketEmployementByTenantIdHandler : IQueryHandler<QueryJiraTicketEmployementByTenantId, GetJiraEmployementTicketDto>
    {
        private readonly IRepository<TenantEmployeeInconmeValidation, string> _tenantJiraValidationRepository;
        private readonly ILogger<QueryPersonaInquiryByTenantIdHandler> _logger;

        public QueryJiraTicketEmployementByTenantIdHandler(
            IRepository<TenantEmployeeInconmeValidation, string> tenantJiraValidationRepository,
            ILogger<QueryPersonaInquiryByTenantIdHandler> logger)
        {

            _tenantJiraValidationRepository = tenantJiraValidationRepository;
            _logger = logger;
        }
        
   
        public async Task<GetJiraEmployementTicketDto> HandleAsync(QueryJiraTicketEmployementByTenantId query)
        {
            Expression<Func<TenantEmployeeInconmeValidation, bool>> expressionJira = (em => em.TenantId == query.TenantId);
            var infoJira = (await _tenantJiraValidationRepository.Get(expressionJira)).FirstOrDefault();

            if (infoJira == null)
            {
                return null;
            }
            var objResult = new GetJiraEmployementTicketDto()
            {
                TenantId = infoJira.TenantId,
                Comments = infoJira.Comments,
                CreatedOn = infoJira.CreatedOn,
                FinalDecision = infoJira.FinalDecision,
                Status = infoJira.Status,

            };
            return objResult;
        }
    }
}

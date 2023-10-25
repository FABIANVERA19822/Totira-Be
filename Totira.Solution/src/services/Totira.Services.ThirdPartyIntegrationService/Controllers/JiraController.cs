using Microsoft.AspNetCore.Mvc;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Jira;
using Totira.Business.ThirdPartyIntegrationService.Queries.Persona;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.ThirdPartyIntegrationService.Controllers
{

    public class JiraController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryJiraTicketEmployementByTenantId, GetJiraEmployementTicketDto> _jiraHandler;
        private readonly ILogger<JiraController> _logger;

        public JiraController(
            IQueryHandler<QueryJiraTicketEmployementByTenantId, GetJiraEmployementTicketDto> jiraHandler,
            ILogger<JiraController> logger
            )
        {
            _jiraHandler = jiraHandler;
            _logger = logger;
        }


        [HttpGet("applicants/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetJiraEmployementTicketDto>> GetInquiry(Guid tenantId)
        {

            return await _jiraHandler.HandleAsync(new QueryJiraTicketEmployementByTenantId(tenantId));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Persona;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.ThirdPartyIntegrationService.Controllers
{
    public class PersonaController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryPersonaInquiryByTenantId, GetPersonaApplicationDto> _personaHandler;
        private readonly ILogger<PersonaController> _logger;

        public PersonaController(
            IQueryHandler<QueryPersonaInquiryByTenantId, GetPersonaApplicationDto> personaHandler,
            ILogger<PersonaController> logger
            )
        {
            _personaHandler = personaHandler;
            _logger = logger;
        }


        [HttpGet("applicants/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetPersonaApplicationDto>> GetInquiry(Guid tenantId)
        {

            return await _personaHandler.HandleAsync(new QueryPersonaInquiryByTenantId(tenantId));
        }
    }
}

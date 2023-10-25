using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.UserService.Controllers
{

    public class ReferralInfoController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto> _getReferralInfoHandler;
        private readonly ILogger<ReferralInfoController> _logger;
        public ReferralInfoController(
            IQueryHandler<QueryAcquaintanceReferralFormInfoByReferralId, GetAcquaintanceReferralFormInfoDto> getReferralInfoHandler,
            ILogger<ReferralInfoController> logger)
        {
            _getReferralInfoHandler = getReferralInfoHandler;
            _logger = logger;
        }

        [HttpGet("{referralId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAcquaintanceReferralFormInfoDto>> Get(Guid referralId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getReferralInfoHandler.HandleAsync(new QueryAcquaintanceReferralFormInfoByReferralId(referralId));
        }
    }
}


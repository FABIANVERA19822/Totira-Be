using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.UserService.Controllers
{

    public class UserFilesController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryDownloadTenantIncomeFile, DownloadTenantFileDto> _downloadTenantIncomeFileQueryHandler;
        private readonly IQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto> _getTenantFilesHandler;
        private readonly ILogger<UserFilesController> _logger;
        public UserFilesController(
            ILogger<UserFilesController> logger,
            IQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto> getTenantFilesHandler,
            IQueryHandler<QueryDownloadTenantIncomeFile, DownloadTenantFileDto> downloadTenantIncomeFileQueryHandler)
        {
            _getTenantFilesHandler = getTenantFilesHandler;
            _logger = logger;
            _downloadTenantIncomeFileQueryHandler = downloadTenantIncomeFileQueryHandler;
        }

        [HttpGet]
        [Route("tenants/{tenantId}/files/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DownloadTenantFileDto>> DownloadTenantFile(Guid tenantId, string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _downloadTenantIncomeFileQueryHandler.HandleAsync(new(fileName, tenantId, null));
        }

        [HttpGet]
        [Route("tenants/{tenantId}/incomes/{incomeId}/files/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DownloadTenantFileDto>> DownloadTenantIncomeFile(Guid tenantId, Guid incomeId, string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _downloadTenantIncomeFileQueryHandler.HandleAsync(new(fileName, tenantId, incomeId));
        }

        [HttpGet("{tenantId}/{incomeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetIncomeFilesDto>> GetVerificationProcessStatus(Guid tenantId, Guid incomeId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _getTenantFilesHandler.HandleAsync(new QueryTenantGetFileByTenantIAndIncomeId(tenantId,incomeId));
        }
    }
}

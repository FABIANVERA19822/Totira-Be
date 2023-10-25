using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.Queries.Files;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.UserService.Controllers
{

    public class UserFilesController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryDownloadTenantIncomeFile, DownloadFileDto> _downloadTenantIncomeFileQueryHandler;
        private readonly IQueryHandler<QueryDownloadLandlordIdentityFile, DownloadFileDto> _downloadLandlordIdentityFileQueryHandler;
        private readonly IQueryHandler<QueryDownloadLandlordClaimFile, DownloadFileDto> _downloadLandlordClaimFileQueryHandler;
        private readonly IQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto> _getTenantFilesHandler;
        private readonly ILogger<UserFilesController> _logger;
        public UserFilesController(
            ILogger<UserFilesController> logger,
            IQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto> getTenantFilesHandler,
            IQueryHandler<QueryDownloadTenantIncomeFile, DownloadFileDto> downloadTenantIncomeFileQueryHandler,
            IQueryHandler<QueryDownloadLandlordIdentityFile, DownloadFileDto> downloadLandlordIdentityFileQueryHandler,
            IQueryHandler<QueryDownloadLandlordClaimFile, DownloadFileDto> downloadLandlordClaimFileQueryHandler)
        {
            _getTenantFilesHandler = getTenantFilesHandler;
            _logger = logger;
            _downloadTenantIncomeFileQueryHandler = downloadTenantIncomeFileQueryHandler;
            _downloadLandlordIdentityFileQueryHandler = downloadLandlordIdentityFileQueryHandler;
            _downloadLandlordClaimFileQueryHandler = downloadLandlordClaimFileQueryHandler;
        }

        [HttpGet]
        [Route("tenants/{tenantId}/files/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DownloadFileDto>> DownloadTenantFile(Guid tenantId, string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _downloadTenantIncomeFileQueryHandler.HandleAsync(new(fileName, tenantId, null));
        }

        [HttpGet]
        [Route("tenants/{tenantId}/incomes/{incomeId}/files/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<DownloadFileDto>> DownloadTenantIncomeFile(Guid tenantId, Guid incomeId, string fileName)
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

        #region Landlord Files
        [HttpGet("landlords/{landlordId}/identityFiles/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DownloadFileDto>> DownloadLandlordIdentityFile(Guid landlordId, string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _downloadLandlordIdentityFileQueryHandler.HandleAsync(new QueryDownloadLandlordIdentityFile(landlordId, fileName));
        }

        [HttpGet("landlords/{landlordId}/claimFiles/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DownloadFileDto>> DownloadLandlordClaimFile(Guid landlordId, string fileName)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _downloadLandlordClaimFileQueryHandler.HandleAsync(new QueryDownloadLandlordClaimFile(landlordId, fileName));
        }
        #endregion
    }
}

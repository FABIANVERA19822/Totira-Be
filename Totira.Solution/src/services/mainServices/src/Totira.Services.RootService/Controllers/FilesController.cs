using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Services.RootService.DTO.Files;
using Totira.Services.RootService.DTO.Files.Request;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;

namespace Totira.Services.RootService.Controllers
{
    [Authorize(Policy = "AppOptions")]
    public class FilesController : DefaultBaseController
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;

        public FilesController(
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions)
        {
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
        }

        [HttpGet]
        [Route("tenants")]
        [Produces("application/pdf", "image/jpeg", "image/png")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetTenantFile([FromQuery] GetTenantFileRequest request)
        {
            string url;

            if (request.IncomeId is not null)
                url = $"{_restClientOptions.User}/UserFiles/tenants/{request.TenantId}/incomes/{request.IncomeId}/files/{request.FileName}";
            else
                url = $"{_restClientOptions.User}/UserFiles/tenants/{request.TenantId}/files/{request.FileName}";

            var file = await _queryRestClient.GetAsync<DownloadTenantFileDto>(url);

            if (file is null || file.StatusCode != HttpStatusCode.OK || file.Content.Content is null)
                return NoContent();

            return File(file.Content.Content, file.Content.ContentType, file.Content.FileName);
        }

        [HttpGet]
        [Route("landlordIdentity")]
        [Produces("application/pdf", "image/jpeg", "image/png")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetLandlordIdentityFile([FromQuery] GetLandlordFileRequest request)
        {
            string url = $"{_restClientOptions.User}/UserFiles/landlords/{request.LandlordId}/identityFiles/{request.FileName}";

            var file = await _queryRestClient.GetAsync<DownloadFileDto>(url);

            if (file is null || file.StatusCode != HttpStatusCode.OK || file.Content.Content is null)
                return NoContent();

            return File(file.Content.Content, file.Content.ContentType, file.Content.FileName);
        }


        [HttpGet]
        [Route("landlordClaims")]
        [Produces("application/pdf", "image/jpeg", "image/png")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetLandlordPropertyClaimFile([FromQuery] GetLandlordFileRequest request)
        {
            string url = $"{_restClientOptions.User}/UserFiles/landlords/{request.LandlordId}/claimFiles/{request.FileName}";

            var file = await _queryRestClient.GetAsync<DownloadFileDto>(url);

            if (file is null || file.StatusCode != HttpStatusCode.OK || file.Content.Content is null)
                return NoContent();

            return File(file.Content.Content, file.Content.ContentType, file.Content.FileName);
        }
    }
}
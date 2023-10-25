namespace Totira.Services.RootService.Controllers
{
    using System;
    using System.IO;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Totira.Services.RootService.Commands;
    using Totira.Services.RootService.DTO;
    using Totira.Services.RootService.Extensions;
    using Totira.Support.Api.Connection;
    using Totira.Support.Api.Controller;
    using Totira.Support.Api.Options;
    using Totira.Support.Application.Messages;
    using Totira.Support.EventServiceBus;

    public class TenantPersonalInfoController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IEventBus _bus;
        private readonly ILogger<TenantPersonalInfoController> _logger;
        private const string API_VERSION = "1";

        public TenantPersonalInfoController(
            IEventBus bus,
            IContextFactory contextFactory,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            ILogger<TenantPersonalInfoController> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _bus = bus;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Method to save image profile in database and fileFolder
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("UploadImageProfile")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] UploadTenantProfileImageDto info)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value,out Guid userIdAux) ? userIdAux : info.TenantId;
            string url = Url.Action("Get", "UserPersonal", new { id = info.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            _logger.LogInformation($"UploadImageProfile has been reached with the userId: {info.TenantId}");

            IFormFile file = info.File.BuildImageFromBase64(info.ImageName, info.ImageType);

            var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            //Command to publish
            var command = new CreateTenantProfileImageCommand()
            {
                TenantId = info.TenantId,
                File = new ProfileImageFile(file.FileName, file.ContentType, stream.ToArray())
            };

            try
            {
                _logger.LogInformation($"Uploadfile has been request with file:{command.File.FileName}");

                if (!VerifyImageExtension(file))
                {
                    return BadRequest(new { message = "The format is not the one allowed" });
                }
                else
                {
                    _logger.LogInformation($"PublishAsync to servicebus has been reached with the userId: {info.TenantId}");
                    await _bus.PublishAsync(context, command);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UploadImageProfile process with userId: {command.TenantId} failed, reason: {ex.Message}");
                return BadRequest(ex.Message);
            }
            return Accepted(context);
        }

        /// <summary>
        /// Method to get image profile 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ProfileImage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetTenantProfileImageDto>> GetTenantProfileImage(Guid id)
        {
            var personalinfo = await _queryRestClient.GetAsync<GetTenantProfileImageDto>($"{_restClientOptions.User}/user/tenant/profileimage/{id}");

            if (personalinfo == null)
                return NotFound();

            return personalinfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppOptions")]
        [HttpPut("UpdateTenantProfileImage")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(20000000)]
        public async Task<IActionResult> UpdateTenantProfileImage([FromForm] UpdateTenantProfileImageDto dto)
        {
            string url = Url.Action("Get", "UserPersonal", new { id = dto.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : dto.TenantId;
            var context = _contextFactory.Create(url, userId);

            _logger.LogInformation($"UpdateTenantProfileImage has been reached with the userId: {dto.TenantId}");

            var stream = new MemoryStream();
            if (dto.File != null) { await dto.File.CopyToAsync(stream); }

            //Command to publish
            var command = new UpdateTenantProfileImageCommand()
            {
                TenantId = dto.TenantId,
                File = dto.File != null ? new ProfileImageFileEmpty(dto.File.FileName, dto.File.ContentType, stream.ToArray()) : default!
            };

            try
            {
                if (dto.File != null && !(VerifyImageExtension(dto.File)))
                {
                    return BadRequest(new { message = "The format is not the one allowed" });
                }
                else
                {
                    _logger.LogInformation($"PublishAsync to servicebus has been reached with the userId: {dto.TenantId}");
                    await _bus.PublishAsync(context, command);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateTenantProfileImage process with userId: {command.TenantId} failed, reason: {ex.Message}");
                return BadRequest(ex.Message);
            }
            return Accepted(context);
        }

        private static bool VerifyImageExtension(IFormFile file)
        {
            var allowedExtension = new string[] { ".jpg", ".png", ".jpeg" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (allowedExtension.Contains(extension))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpGet("ProfileProgress/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Dictionary<string, int>>> GetTenantProfileProgress(Guid id)
        {
            var progress = await _queryRestClient.GetAsync<Dictionary<string, int>>($"{_restClientOptions.User}/user/tenant/profileprogress/{id}");

            if (progress == null)
                return NotFound();

            return progress;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("ProfileFunnel/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, bool>>> GetTenantProfileFunnel(Guid id)
        {
            var funnel = await _queryRestClient.GetAsync<Dictionary<string, bool>>($"{_restClientOptions.User}/user/tenant/profilefunnel/{id}");

            if (funnel == null)
                return NotFound();

            return funnel;
        }
    }
}
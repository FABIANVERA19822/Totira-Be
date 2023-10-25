using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Services.RootService.DTO.GroupTenant;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;

namespace Totira.Services.RootService.Controllers;

[Authorize]
public class GroupTenantController : DefaultBaseController
{
    private readonly IQueryRestClient _queryRestClient;
    private readonly RestClientOptions _restClientOptions;

    public GroupTenantController(
        IQueryRestClient queryRestClient,
        IOptions<RestClientOptions> restClientOptions)
    {
        _queryRestClient = queryRestClient;
        _restClientOptions = restClientOptions.Value;
    }

    [HttpGet("{mainTenantId}/verified-status")]
    [ProducesResponseType(typeof(GetTenantGroupVerifiedProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
    public async Task<ActionResult<GetTenantGroupVerifiedProfileDto>> GetVerifiedStatus(Guid mainTenantId)
    {
        var url = $"{_restClientOptions.User}/group/main-tenant/{mainTenantId}/verified-status";
        var result = await _queryRestClient.GetAsync<GetTenantGroupVerifiedProfileDto>(url);

        if (result is null)
            return NoContent();

        return result;
    }

    [HttpGet("{mainTenantId}/dashboard")]
    [ProducesResponseType(typeof(GetTenantGroupDashboardProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
    public async Task<ActionResult<GetTenantGroupDashboardProfileDto>> GetDashboard(Guid mainTenantId)
    {
        var url = $"{_restClientOptions.User}/group/main-tenant/{mainTenantId}/dashboard";
        var result = await _queryRestClient.GetAsync<GetTenantGroupDashboardProfileDto>(url);

        if (result is null)
            return NoContent();

        return result;
    }
}
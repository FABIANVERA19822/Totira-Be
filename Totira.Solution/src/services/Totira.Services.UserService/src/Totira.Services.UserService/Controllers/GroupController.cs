using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.DTO.GroupTenant;
using Totira.Bussiness.UserService.DTO.GroupTenant.Certn;
using Totira.Bussiness.UserService.Queries.Group;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.UserService.Controllers;

public class GroupController : DefaultBaseController
{
    private readonly IQueryHandler<QueryGetGroupApplicantsInfoByTenantId, GetGroupApplicantsInfoByTenantIdDto> _getGroupApplicantsInfoByMainTenantIdQueryHandler;
    private readonly IQueryHandler<QueryGetGroupVerifiedProfilebyTenantId, GetTenantGroupVerifiedProfileDto> _getTenantGroupVerifiedProfileByTenantIdQueryHandler;
    private readonly IQueryHandler<QueryGetGroupDashboardProfileByTenantId, GetTenantGroupDashboardProfileDto> _getTenantGroupDashboardProfileByTenantIdQueryHandler;

    public GroupController(
        IQueryHandler<QueryGetGroupApplicantsInfoByTenantId, GetGroupApplicantsInfoByTenantIdDto> getGroupApplicantsInfoByMainTenantIdQueryHandler,
        IQueryHandler<QueryGetGroupVerifiedProfilebyTenantId, GetTenantGroupVerifiedProfileDto> getTenantGroupVerifiedProfileByTenantIdQueryHandler,
        IQueryHandler<QueryGetGroupDashboardProfileByTenantId, GetTenantGroupDashboardProfileDto> getTenantGroupDashboardProfileByTenantIdQueryHandler)
    {
        _getGroupApplicantsInfoByMainTenantIdQueryHandler = getGroupApplicantsInfoByMainTenantIdQueryHandler;
        _getTenantGroupVerifiedProfileByTenantIdQueryHandler = getTenantGroupVerifiedProfileByTenantIdQueryHandler;
        _getTenantGroupDashboardProfileByTenantIdQueryHandler = getTenantGroupDashboardProfileByTenantIdQueryHandler;
    }

    [HttpGet("main-tenant/{tenantId}/certn/request-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetGroupApplicantsInfoByTenantIdDto>> GetGroupInformation(Guid tenantId)
    {
        var query = new QueryGetGroupApplicantsInfoByTenantId() { TenantId = tenantId };
        var result = await _getGroupApplicantsInfoByMainTenantIdQueryHandler.HandleAsync(query);
        return Ok(result);
    }

    [HttpGet("main-tenant/{tenantId}/verified-status")]
    public async Task<ActionResult<GetTenantGroupVerifiedProfileDto>> GetGroupVerifiedStatus(Guid tenantId)
    {
        var query = new QueryGetGroupVerifiedProfilebyTenantId() { TenantId = tenantId };
        var result = await _getTenantGroupVerifiedProfileByTenantIdQueryHandler.HandleAsync(query);
        return Ok(result);
    }

    [HttpGet("main-tenant/{tenantId}/dashboard")]
    [ProducesResponseType(typeof(GetTenantGroupDashboardProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetTenantGroupDashboardProfileDto>> GetGroupDashboard(Guid tenantId)
    {
        var query = new QueryGetGroupDashboardProfileByTenantId() { TenantId = tenantId };
        var result = await _getTenantGroupDashboardProfileByTenantIdQueryHandler.HandleAsync(query);
        return Ok(result);
    }
}
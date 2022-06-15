using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;
using SwarmPortal.SQLite;

namespace SwarmPortal.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IItemDictionaryGeneratorProvider<ILinkItem> _hostLinkProvider;
    private readonly ILinkAccessor linkAccessor;
    private readonly IRoleAccessor roleAccessor;
    private readonly IGroupAccessor groupAccessor;

    public AdminController(
        ILogger<AdminController> logger, 
        IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider,
        ILinkAccessor linkAccessor,
        IRoleAccessor roleAccessor,
        IGroupAccessor groupAccessor
    )
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
        this.linkAccessor = linkAccessor;
        this.roleAccessor = roleAccessor;
        this.groupAccessor = groupAccessor;
    }
    [HttpGet("All/Links")]
    public async Task<ActionResult<IEnumerable<ILink>>> GetDatabaseLinks(CancellationToken ct)
    {
        
        return await linkAccessor.GetLinks(ct).OkAsync();
    }
    [HttpGet("{group}/Links")]
    public async Task<ActionResult<IEnumerable<ILink>>> GetDatabaseLinksInGroup(string group, CancellationToken ct)
    {
        return await linkAccessor.GetLinksForGroup(group, ct).OkAsync();
    }
    [HttpGet("All/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILink>>> GetDatabaseLinksForRole(string role, CancellationToken ct)
    {
        return await linkAccessor.GetLinksForRole(role, ct).OkAsync();
    }

    [HttpGet("{group}/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILink>>> GetDatabaseLinksForRoleInGroup(string role, string group, CancellationToken ct)
    {
        return await linkAccessor.GetLinksForGroupAndRole(role, group, ct).OkAsync();
    }
    [HttpGet("Roles")]
    public async Task<ActionResult<IEnumerable<IRole>>> GetDatabaseRoles(CancellationToken ct)
    {
        return await roleAccessor.GetRoles(ct).OkAsync();
    }
    [HttpPost("AddRole/{role}")]
    public async Task<ActionResult<string>> AddRole([FromRoute] string role, CancellationToken ct)
    {
        await roleAccessor.AddRole(role, ct);
        return role.Ok();
    }
    [HttpDelete("DeleteRole/{roleId}")]
    public async Task<ActionResult> DeleteRole([FromRoute] ulong roleId, CancellationToken ct)
    {
        await roleAccessor.DeleteRole(roleId, ct);
        return Ok();
    }
    [HttpGet("Groups")]
    public async Task<ActionResult<IEnumerable<IGroup>>> GetDatabaseGroups(CancellationToken ct)
    {
        return await groupAccessor.GetGroups(ct).OkAsync();
    }
    [HttpPost("AddGroup/{group}")]
    public async Task<ActionResult<string>> AddGroup([FromRoute] string group, CancellationToken ct)
    {
        await groupAccessor.AddGroup(group);
        return group.Ok();
    }
    [HttpDelete("DeleteGroup/{groupId}")]
    public async Task<ActionResult> DeleteGroup([FromRoute] ulong groupId, CancellationToken ct)
    {
        await groupAccessor.DeleteGroup(groupId);
        return Ok();
    }
    [HttpPost("AddLink")]
    public async Task<ActionResult<ILinkItem>> AddLink([FromBody] CommonLinkItem link, CancellationToken ct)
    {
        ILinkItem item = link;
        await linkAccessor.AddLink(item);
        return item.Ok();
    }
    [HttpPost("AddLinkRole/{linkId}/{role}")]
    public async Task<ActionResult<string>> AddLinkRole(ulong linkId, string role, CancellationToken ct)
    {
        await linkAccessor.AddLinkRole(linkId, role);
        return role.Ok();
    }
    [HttpDelete("DeleteLink/{linkId}")]
    public async Task<ActionResult> DeleteLink(ulong linkId, CancellationToken ct)
    {
        await linkAccessor.DeleteLink(linkId);
        return Ok();
    }
    [HttpDelete("DeleteLinkRole/{linkId}/{role}")]
    public async Task<ActionResult> DeleteLinkRole(ulong linkId, string role, CancellationToken ct)
    {
        await linkAccessor.DeleteLinkRole(linkId, role);
        return Ok();
    }
}

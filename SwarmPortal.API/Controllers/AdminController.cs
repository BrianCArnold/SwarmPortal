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
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinks(CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinks());
    }
    [HttpGet("{group}/Links")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksInGroup(string group, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForGroup(group));
    }
    [HttpGet("All/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksForRole(string role, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForRole(role));
    }

    [HttpGet("{group}/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksForRoleInGroup(string role, string group, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForGroupAndRole(role, group));
    }
    [HttpGet("Roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetDatabaseRoles(CancellationToken ct)
    {
        return Ok(await roleAccessor.GetRoles());
    }
    [HttpPost("AddRole/{role}")]
    public async Task<ActionResult<string>> AddRole([FromRoute] string role, CancellationToken ct)
    {
        await roleAccessor.AddRole(role);
        return Ok(role);
    }
    [HttpDelete("DeleteRole/{role}")]
    public async Task<ActionResult> DeleteRole([FromRoute] string role, CancellationToken ct)
    {
        await roleAccessor.DeleteRole(role);
        return Ok();
    }
    [HttpGet("Groups")]
    public async Task<ActionResult<IEnumerable<string>>> GetDatabaseGroups(CancellationToken ct)
    {
        return Ok(await groupAccessor.GetGroups());
    }
    [HttpPost("AddGroup/{group}")]
    public async Task<ActionResult<string>> AddGroup([FromRoute] string group, CancellationToken ct)
    {
        await groupAccessor.AddGroup(group);
        return Ok(group);
    }
    [HttpDelete("DeleteGroup/{group}")]
    public async Task<ActionResult> DeleteGroup([FromRoute] string group, CancellationToken ct)
    {
        await groupAccessor.DeleteGroup(group);
        return Ok();
    }
    [HttpPost("AddLink")]
    public async Task<ActionResult<ILinkItem>> AddLink([FromBody] CommonLinkItem link, CancellationToken ct)
    {
        await linkAccessor.AddLink(link);
        return Ok(link);
    }
    [HttpPost("AddLinkRole/{linkGroup}/{linkName}/{role}")]
    public async Task<ActionResult<string>> AddLinkRole(string linkName, string linkGroup, string role, CancellationToken ct)
    {
        await linkAccessor.AddLinkRole(linkName, linkGroup, role);
        return Ok(role);
    }
    [HttpDelete("DeleteLink/{linkGroup}/{linkName}")]
    public async Task<ActionResult> DeleteLink(string linkName, string linkGroup, CancellationToken ct)
    {
        await linkAccessor.DeleteLink(linkName, linkGroup);
        return Ok();
    }
    [HttpDelete("DeleteLinkRole/{linkGroup}/{linkName}/{role}")]
    public async Task<ActionResult> DeleteLinkRole(string linkName, string linkGroup, string role, CancellationToken ct)
    {
        await linkAccessor.DeleteLinkRole(linkName, linkGroup, role);
        return Ok();
    }
}

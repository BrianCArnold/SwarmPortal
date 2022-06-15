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
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinks(string host, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinks());
    }
    [HttpGet("{group}/Links")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksInGroup(string host, string group, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForGroup(group));
    }
    [HttpGet("All/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksForRole(string host, string role, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForRole(role));
    }

    [HttpGet("{group}/LinksFor/{role}")]
    public async Task<ActionResult<IEnumerable<ILinkItem>>> GetDatabaseLinksForRoleInGroup(string host, string role, string group, CancellationToken ct)
    {
        return Ok(await linkAccessor.GetLinksForGroupAndRole(role, group));
    }
    [HttpGet("Roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetDatabaseRoles(string host, CancellationToken ct)
    {
        return Ok(await roleAccessor.GetRoles());
    }
    [HttpGet("Groups")]
    public async Task<ActionResult<IEnumerable<string>>> GetDatabaseGroups(string host, CancellationToken ct)
    {
        return Ok(await groupAccessor.GetGroups());
    }
}

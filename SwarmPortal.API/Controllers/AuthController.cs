using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;
using SwarmPortal.SQLite;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IItemDictionaryGeneratorProvider<ILinkItem> _hostLinkProvider;
    private readonly IAuthConfig _authConfig;
    private readonly IRoleAccessor _roleAccessor;
    private readonly IGroupAccessor _groupAccessor;

    public AuthController(
        ILogger<AuthController> logger, 
        IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider, 
        IAuthConfig authConfig,
        IRoleAccessor roleAccessor, 
        IGroupAccessor groupAccessor)
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
        _authConfig = authConfig;
        _roleAccessor = roleAccessor;
        _groupAccessor = groupAccessor;
    }


    [HttpGet("Config")]
    public Task<ActionResult<IAuthConfig>> Get(CancellationToken ct)
    {
        return Task.FromResult(_authConfig.Ok());
    }
    [Authorize]
    [HttpGet("ProcessLogin")]
    public async Task<IActionResult> ProcessLogin(CancellationToken ct)
    {
        await Task.WhenAll(AddDetectedGroups(ct), AddDetectedRoles(ct));
        return this.Ok();
    }
    private async Task AddDetectedRoles(CancellationToken ct)
    {
        var currentRoles = await _roleAccessor.GetRoles(ct);
        var currentRoleSet = currentRoles.Select(r => r.Name).ToHashSet();
        var userRoles = this.User.Claims.GetRoles();
        foreach(var role in userRoles)
        {
            if (!currentRoleSet.Contains(role))
            {
                var newRole = await _roleAccessor.AddRole(role, ct);
                await _roleAccessor.DisableRole(newRole.Id, ct);
            }
        }
    }
    private async Task AddDetectedGroups(CancellationToken ct)
    {
        var currentGroups = await _groupAccessor.GetGroups();
        var currentGroupSet = currentGroups.Select(g => g.Name).ToHashSet();
        var dictionaryGenerator = _hostLinkProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionaryWithRoles(ct, User.Claims.GetRoles());
        foreach (var group in dictionary.Keys)
        {
            if (!currentGroupSet.Contains(group))
            {
                var newGroup = await _groupAccessor.AddGroup(group, ct);
                await _groupAccessor.DisableGroup(newGroup.Id, ct);
            }
        }
    }
}

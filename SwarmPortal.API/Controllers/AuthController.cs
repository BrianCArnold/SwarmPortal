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

    public AuthController(
        ILogger<AuthController> logger, 
        IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider, 
        IAuthConfig authConfig,
        IRoleAccessor roleAccessor)
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
        _authConfig = authConfig;
        _roleAccessor = roleAccessor;
    }


    [HttpGet("Config")]
    public Task<ActionResult<IAuthConfig>> Get(CancellationToken ct)
    {
        return Task.FromResult(_authConfig.Ok());
    }
    [HttpGet("ProcessLogin")]
    public async Task<IActionResult> ProcessLogin(CancellationToken ct)
    {
        var currentRoles = await _roleAccessor.GetRoles(ct);
        var currentRoleSet = currentRoles.ToHashSet();
        var userRoles = this.User.Claims.GetRoles();
        foreach(var role in userRoles)
        {
            if (!userRoles.Contains(role))
            {
                var newRole = await _roleAccessor.AddRole(role, ct);
                await _roleAccessor.DisableRole(newRole.Id, ct);
            }
        }
        return this.Ok();
    }
}

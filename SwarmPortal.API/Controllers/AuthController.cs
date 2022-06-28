using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;
using SwarmPortal.Context;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IItemDictionaryProvider<ILinkItem> _dictionaryProvider;
    private readonly IItemOrderingProvider<ILinkItem> _orderingProvider;
    private readonly IItemRoleFilteringProvider<ILinkItem> _roleFilteringProvider;
    private readonly ICoalescedItemProvider<ILinkItem> _coalescedItemProvider;

    private readonly IAuthConfig _authConfig;
    private readonly IRoleAccessor _roleAccessor;
    private readonly IGroupAccessor _groupAccessor;

    public AuthController(
        ILogger<AuthController> logger,  
        IItemDictionaryProvider<ILinkItem> dictionaryProvider,
        IItemOrderingProvider<ILinkItem> orderingProvider,
        IItemRoleFilteringProvider<ILinkItem> roleFilteringProvider,
        ICoalescedItemProvider<ILinkItem> coalescedItemProvider, 
        IAuthConfig authConfig,
        IRoleAccessor roleAccessor, 
        IGroupAccessor groupAccessor)
    {
        _logger = logger;
        _dictionaryProvider = dictionaryProvider;
        _orderingProvider = orderingProvider;
        _roleFilteringProvider = roleFilteringProvider;
        _coalescedItemProvider = coalescedItemProvider;
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
        var currentGroup = await _groupAccessor.GetGroups();
        var currentGroupSet = currentGroup.Select(g => g.Name).ToHashSet();
        var links = _coalescedItemProvider.GetItems(ct);

        var dictionary = await _dictionaryProvider.GetDictionaryAsync(links, ct);
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

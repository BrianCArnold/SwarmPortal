using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusesController : ControllerBase
{
    private readonly ILogger<StatusesController> _logger;
    private readonly IItemDictionaryProvider<IStatusItem> _dictionaryProvider;
    private readonly IItemOrderingProvider<IStatusItem> _orderingProvider;
    private readonly IItemRoleFilteringProvider<IStatusItem> _roleFilteringProvider;
    private readonly ICoalescedItemProvider<IStatusItem> _coalescedItemProvider;

    public StatusesController(ILogger<StatusesController> logger, 
        IItemDictionaryProvider<IStatusItem> dictionaryProvider,
        IItemOrderingProvider<IStatusItem> orderingProvider,
        IItemRoleFilteringProvider<IStatusItem> roleFilteringProvider,
        ICoalescedItemProvider<IStatusItem> coalescedItemProvider)
    {
        _logger = logger;
        _dictionaryProvider = dictionaryProvider;
        _orderingProvider = orderingProvider;
        _roleFilteringProvider = roleFilteringProvider;
        _coalescedItemProvider = coalescedItemProvider;
    }

    
    [HttpGet("Public")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IStatusItem>>>> Get(CancellationToken ct)
     => Ok(await GetStatusesInternal(ct));
    
    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IStatusItem>>>> GetAll(CancellationToken ct)
     => Ok(await GetStatusesInternal(ct));
    private async Task<Dictionary<string, IEnumerable<IStatusItem>>> GetStatusesInternal(CancellationToken ct)
    {
        var userRoles = User.Claims.GetRoles();
        var items = _coalescedItemProvider.GetItems(ct);
        var filteredItems = _roleFilteringProvider.FilterItemsByRoles(items, userRoles, ct);
        var orderedItems = _orderingProvider.OrderItems(filteredItems);
        var dictionary = await _dictionaryProvider.GetDictionaryAsync(orderedItems, ct);
        return await Task.FromResult(dictionary);
    }
    
}

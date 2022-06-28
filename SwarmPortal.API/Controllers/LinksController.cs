using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;
[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;
    private readonly IItemDictionaryProvider<ILinkItem> _dictionaryProvider;
    private readonly IItemOrderingProvider<ILinkItem> _orderingProvider;
    private readonly IItemRoleFilteringProvider<ILinkItem> _roleFilteringProvider;
    private readonly ICoalescedItemProvider<ILinkItem> _coalescedItemProvider;

    public LinksController(
        ILogger<LinksController> logger,  
        IItemDictionaryProvider<ILinkItem> dictionaryProvider,
        IItemOrderingProvider<ILinkItem> orderingProvider,
        IItemRoleFilteringProvider<ILinkItem> roleFilteringProvider,
        ICoalescedItemProvider<ILinkItem> coalescedItemProvider
        )
    {
        _logger = logger;
        _dictionaryProvider = dictionaryProvider;
        _orderingProvider = orderingProvider;
        _roleFilteringProvider = roleFilteringProvider;
        _coalescedItemProvider = coalescedItemProvider;
    }

    [HttpGet("Public")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<ILinkItem>>>> Get(CancellationToken ct)
     => Ok(await GetLinksInternal(ct));

    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<ILinkItem>>>> GetAll(CancellationToken ct)
     => Ok(await GetLinksInternal(ct));

    private async Task<Dictionary<string, IEnumerable<ILinkItem>>> GetLinksInternal(CancellationToken ct)
    {
        var userRoles = User.Claims.GetRoles();
        var items = _coalescedItemProvider.GetItems(ct);
        var filteredItems = _roleFilteringProvider.FilterItemsByRoles(items, userRoles, ct);
        var orderedItems = _orderingProvider.OrderItems(filteredItems);
        var dictionary = await _dictionaryProvider.GetDictionaryAsync(orderedItems, ct);      
        return await Task.FromResult(dictionary);
    }

}

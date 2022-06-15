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
    private readonly IItemDictionaryGeneratorProvider<ILinkItem> _hostLinkProvider;

    public LinksController(
        ILogger<LinksController> logger, 
        IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider
        )
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
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
        var dictionaryGenerator = _hostLinkProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionaryWithRoles(ct, userRoles);
        
      
        return await Task.FromResult(dictionary);
    }

}

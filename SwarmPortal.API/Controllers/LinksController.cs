using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;
    private readonly IItemDictionaryGeneratorProvider<ILinkItem> _hostLinkProvider;

    public LinksController(ILogger<LinksController> logger, IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider)
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
    }

    [HttpGet("All")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<ILinkItem>>>> Get(CancellationToken ct)
    {
        var dictionaryGenerator = _hostLinkProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionary(ct);
        return await Task.FromResult(dictionary);
    }
}

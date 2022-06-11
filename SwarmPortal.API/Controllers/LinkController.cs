using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;
    private readonly IItemDictionaryGeneratorProvider<IGroupableLinkItem> _hostLinkProvider;

    public LinksController(ILogger<LinksController> logger, IItemDictionaryGeneratorProvider<IGroupableLinkItem> hostLinkProvider)
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IGroupableLinkItem>>>> Get(CancellationToken ct)
    {
        var dictionaryGenerator = _hostLinkProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionary(ct);
        return await Task.FromResult(dictionary);
    }
}

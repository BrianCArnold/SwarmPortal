using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HostsController : ControllerBase
{
    private readonly ILogger<HostsController> _logger;
    private readonly IItemDictionaryGeneratorProvider<IGroupableHostItem> _hostGroupProvider;

    public HostsController(ILogger<HostsController> logger, IItemDictionaryGeneratorProvider<IGroupableHostItem> hostGroupProvider)
    {
        _logger = logger;
        _hostGroupProvider = hostGroupProvider;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IGroupableHostItem>>>> Get(CancellationToken ct)
    {
        var dictionaryGenerator = _hostGroupProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionary(ct);
        return await Task.FromResult(dictionary);
    }
}

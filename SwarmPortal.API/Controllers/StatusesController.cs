using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusesController : ControllerBase
{
    private readonly ILogger<StatusesController> _logger;
    private readonly IItemDictionaryGeneratorProvider<IStatusItem> _hostGroupProvider;

    public StatusesController(ILogger<StatusesController> logger, IItemDictionaryGeneratorProvider<IStatusItem> hostGroupProvider)
    {
        _logger = logger;
        _hostGroupProvider = hostGroupProvider;
    }

    [HttpGet("All")]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IStatusItem>>>> Get(CancellationToken ct)
    {
        var dictionaryGenerator = _hostGroupProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionary(ct);
        return await Task.FromResult(dictionary);
    }
}

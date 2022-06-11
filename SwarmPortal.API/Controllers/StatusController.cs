using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IItemDictionaryGeneratorProvider<IStatusItem> _hostGroupProvider;

    public StatusController(ILogger<StatusController> logger, IItemDictionaryGeneratorProvider<IStatusItem> hostGroupProvider)
    {
        _logger = logger;
        _hostGroupProvider = hostGroupProvider;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, IEnumerable<IStatusItem>>>> Get(CancellationToken ct)
    {
        var dictionaryGenerator = _hostGroupProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionary(ct);
        return await Task.FromResult(dictionary);
    }
}

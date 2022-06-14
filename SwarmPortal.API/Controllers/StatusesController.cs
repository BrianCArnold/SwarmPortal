using Microsoft.AspNetCore.Authorization;
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
        var userRoles = User.Claims.GetRoles();
        var dictionaryGenerator = _hostGroupProvider.GetDictionaryGeneratorAsync(ct);
        var dictionary = await dictionaryGenerator.GetDictionaryWithRoles(ct, userRoles);


        // Console.WriteLine(userRoles.StringJoin(","));
        return await Task.FromResult(dictionary);
    }
    
}

using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IItemDictionaryGeneratorProvider<ILinkItem> _hostLinkProvider;
    private readonly IAuthConfig _authConfig;

    public AuthController(ILogger<AuthController> logger, IItemDictionaryGeneratorProvider<ILinkItem> hostLinkProvider, IAuthConfig authConfig)
    {
        _logger = logger;
        _hostLinkProvider = hostLinkProvider;
        _authConfig = authConfig;
    }


    [HttpGet("Config")]
    public Task<ActionResult<IAuthConfig>> Get(CancellationToken ct)
    {
        return Task.FromResult(_authConfig.Ok());
    }
}

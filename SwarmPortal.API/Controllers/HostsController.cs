using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HostsController : ControllerBase
{
    private readonly ILogger<HostsController> _logger;
    private readonly IHostGroupProvider _hostGroupProvider;

    public HostsController(ILogger<HostsController> logger, IHostGroupProvider hostGroupProvider)
    {
        _logger = logger;
        _hostGroupProvider = hostGroupProvider;
    }

    [HttpGet(Name = "All")]
    public async Task<ActionResult<IGroupedHosts>> Get()
    {
        return _hostGroupProvider.GetHostGroupsAsync();
    }
}

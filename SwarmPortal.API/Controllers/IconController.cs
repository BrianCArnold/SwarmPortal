using System.Web;
using Microsoft.AspNetCore.Mvc;
using SwarmPortal.Common;

namespace SwarmPortal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class IconController : ControllerBase
{
    private readonly IIconProvider iconProvider;

    public IconController(IIconProvider iconProvider)
    {
        this.iconProvider = iconProvider;
    }

    [HttpGet("{uri}")]
    public async Task<ActionResult<FileStream?>> Get(string uri, CancellationToken ct = default)
    {
        var decodedUriString = HttpUtility.UrlDecode(uri);
        var decodedUri = new Uri(decodedUriString);
        var icon = await iconProvider.GetIcon(decodedUri, ct);
        if (icon == null)
        {
            return NotFound();
        }
        return File(icon, "image/png");
    }
}
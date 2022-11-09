using Microsoft.Net.Http.Headers;
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

    // [ResponseCache(VaryByHeader = "User-Agent", Duration = 300)]
    [HttpGet("{uri}")]
    public async Task<ActionResult<Stream?>> Get(string uri, CancellationToken ct = default)
    {
        await Task.Delay(1000);
        var decodedUriString = HttpUtility.UrlDecode(uri);
        var decodedUri = new Uri(decodedUriString);
        var icon = await iconProvider.GetIcon(decodedUri, ct);
        if (icon.IsSuccess)
        {
            Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromMinutes(5)
            };
            return File(icon.IconStream!, "image/png");
        }
        else 
        {
            Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromMinutes(10)
            };
            return Redirect("failure");
        }
        
    }

    [ResponseCache(VaryByHeader = "User-Agent", Duration = OneDayInSeconds)]
    [HttpGet("failure")]
    public async Task<ActionResult<Stream?>> Failure(CancellationToken ct = default)
    {
        await Task.Delay(1000);
        Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(1)
        };
        var icoStream = await System.IO.File.ReadAllBytesAsync("StaticFiles/defaultIco.png");
        return File(icoStream, "image/png");
    }
    private const int OneDayInSeconds = 60*60*24;
}

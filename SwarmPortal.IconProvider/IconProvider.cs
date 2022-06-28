namespace SwarmPortal.IconProvider;

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwarmPortal.Common;
using SwarmPortal.Context;
public class IconProvider : IIconProvider
{
    private readonly IUriIconAccessor _uriIconAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;
    private readonly ILogger<IconProvider> _logger;
    private readonly DirectoryInfo _iconCacheDirectory;
    private static readonly Dictionary<Uri, IUriIcon> _uriCache= new Dictionary<Uri, IUriIcon>();

    public IconProvider(IUriIconAccessor uriIconAccessor, IHttpContextAccessor httpContextAccessor, ILogger<IconProvider> logger)
    {
        Directory.CreateDirectory("persist");
        _iconCacheDirectory = Directory.CreateDirectory("persist/iconCache");
        _uriIconAccessor = uriIconAccessor;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public async ValueTask<Stream> GetIcon(Uri uri, CancellationToken ct)
    {
        var uriIcon = await GetUriIcon(uri, ct);
        if (uriIcon is null)
        {
            _logger.LogError("Icon not found for uri {uri}, providing default.", uri);
            byte[] data = Convert.FromBase64CharArray(unknownIconCharBase64, 0, unknownIconCharBase64.Length);
            return new MemoryStream(data);
        }
        if (uriIcon.RetrievedDate < DateTime.UtcNow.AddHours(-1))
        {
            await EjectUriIcon(uri, ct);
            uriIcon = await GetUriIcon(uri, ct);
        }
        var ext = Path.GetExtension(uriIcon.Icon.AbsolutePath);
        var fileName = $"{uriIcon.Id}{ext}";
        var filePath = Path.Combine(_iconCacheDirectory.FullName, fileName);
        if (File.Exists(filePath))
        {
            return File.OpenRead(filePath);
        }
        else
        {
            try 
            {
                var Headers = _httpContextAccessor.HttpContext!.Request.Headers;
                Console.WriteLine(Headers);
                var iconStream = await _httpClient.GetStreamAsync(uriIcon.Icon, ct);
                var fileStream = File.Create(filePath);
                await iconStream.CopyToAsync(fileStream, ct);
                fileStream.Close();
                return File.OpenRead(filePath);
            }
            catch(Exception ex)
            {
                _logger.LogError("Icon not found for uri {uri}, providing default.", uri, ex.Message, ex.StackTrace);
                byte[] data = Convert.FromBase64CharArray(unknownIconCharBase64, 0, unknownIconCharBase64.Length);
                return new MemoryStream(data);
            }
        }
    }

    private async Task EjectUriIcon(Uri uri, CancellationToken ct)
    {
        _uriCache.Remove(uri);
        await _uriIconAccessor.DeleteUriIcon(uri, ct);
    }
    private async Task<IUriIcon?> GetUriIcon(Uri uri, CancellationToken ct)
    {
        if (_uriCache.ContainsKey(uri))
        {
            return _uriCache[uri];
        }
        else if (await _uriIconAccessor.ContainsUriIcon(uri, ct))
        {
            var uriIcon = await _uriIconAccessor.GetUriIcon(uri, ct);
            _uriCache[uri] = uriIcon;
            return uriIcon;
        }
        else 
        {
            var iconUri = await DetermineUriIcon(uri, ct);
            if (iconUri is null)
            {
                return null;
            }
            var uriIcon = await _uriIconAccessor.UpsertUriIcon(uri, iconUri, ct);
            _uriCache[uri] = uriIcon;
            return uriIcon;
        }
        
    }
    private static async Task<Uri?> DetermineUriIcon(Uri uri, CancellationToken ct)
    {
        HttpClient cli = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            MaxAutomaticRedirections = 10
        });
        
        
        string favPath;
        
        
        var response = await cli.GetAsync(uri);
        //If it's going strictly more or strictly less precise than the original uri, we'll use it.
        //If it's like "app.domain.com" to "login.domain.com", we don't use it.
        if (!response.RequestMessage.RequestUri.Host.Contains(uri.Host) && !uri.Host.Contains(response.RequestMessage.RequestUri.Host))
        {
            return null;
        }
        var html = await response.Content.ReadAsStringAsync();
        
        string[] matchers = new[]
        {
            "<link[^>]+rel=['\"][^'\"]*icon[^'\"]*['\"][^>]+href=['\"]([^'\"]+)['\"][^>]*>",
            "<link[^>]+href=['\"]([^'\"]+)['\"][^>]+rel=['\"][^'\"]*icon[^'\"]*['\"][^>]*>"
        };
        var favPathMatches = matchers.SelectMany(m => Regex.Matches(html, m)).ToList();
        if (favPathMatches.Any(m => m.Success))
        {
            favPath = favPathMatches.First(m => m.Success).Groups[1].Value;
        }
        else
        {
            favPath = "/favicon.ico";
        }
        
	    return new Uri(response.RequestMessage!.RequestUri!, favPath);
    }
    //TODO: Move to actual png file. This is just to get it working for now.
    private const string unknownIconBase64 = "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";
    private static readonly char[] unknownIconCharBase64 = unknownIconBase64.ToArray();
}

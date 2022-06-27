namespace SwarmPortal.IconProvider;

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SwarmPortal.Common;
using SwarmPortal.Context;
public class IconProvider : IIconProvider
{
    private readonly IUriIconAccessor _uriIconAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpClient _httpClient;
    private readonly DirectoryInfo _iconCacheDirectory;
    private Dictionary<Uri, IUriIcon> _uriCache;

    public IconProvider(IUriIconAccessor uriIconAccessor, IHttpContextAccessor httpContextAccessor)
    {
        Directory.CreateDirectory("persist");
        _iconCacheDirectory = Directory.CreateDirectory("persist/iconCache");
        _uriCache = new Dictionary<Uri, IUriIcon>();
        _uriIconAccessor = uriIconAccessor;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = new HttpClient();
    }

    public async ValueTask<FileStream?> GetIcon(Uri uri, CancellationToken ct)
    {
        var uriIcon = await GetUriIcon(uri, ct);
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
                var authHeader = _httpContextAccessor.HttpContext!.Request.Headers.Authorization;
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authHeader);
                Console.WriteLine(authHeader);
                var iconStream = await _httpClient.GetStreamAsync(uriIcon.Icon, ct);
                var fileStream = File.Create(filePath);
                await iconStream.CopyToAsync(fileStream, ct);
                fileStream.Close();
                return File.OpenRead(filePath);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }

    private async Task EjectUriIcon(Uri uri, CancellationToken ct)
    {
        _uriCache.Remove(uri);
        await _uriIconAccessor.DeleteUriIcon(uri, ct);
    }
    private async Task<IUriIcon> GetUriIcon(Uri uri, CancellationToken ct)
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
            var uriIcon = await _uriIconAccessor.UpsertUriIcon(uri, iconUri, ct);
            _uriCache[uri] = uriIcon;
            return uriIcon;
        }
        
    }
    private async Task<Uri> DetermineUriIcon(Uri uri, CancellationToken ct)
    {
        HttpClient cli = new HttpClient();
        
        
        string favPath;
        
        var html = await cli.GetStringAsync(uri);
        string[] matchers = new[]
        {
            "<link[^>]+rel=['\"]icon['\"][^>]+href=['\"]([^'\"]+)['\"][^>]*>",
            "<link[^>]+href=['\"]([^'\"]+)['\"][^>]+rel=['\"]icon['\"][^>]*>"
        };
        var favPathMatch = matchers.Select(m => Regex.Match(html, m)).FirstOrDefault(m => m.Success);
        if (favPathMatch != null)
        {
            favPath = favPathMatch.Groups[1].Value;
        }
        else 
        {
            favPath = "/favicon.ico";
        }
        
	    return new Uri(uri, favPath);
    }
}

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

    public async ValueTask<IconSuccess> GetIcon(Uri uri, CancellationToken ct)
    {
        var uriIcon = await GetUriIcon(uri, ct);
        if (uriIcon is null)
        {
            _logger.LogError("Icon not found for uri {uri}, providing default.", uri);
            return IconSuccess.Failure;
        }
        var ext = Path.GetExtension(uriIcon.Icon.AbsolutePath);
        var fileName = $"{uriIcon.Id}{ext}";
        var filePath = Path.Combine(_iconCacheDirectory.FullName, fileName);
        if (!File.Exists(filePath))
        {
            try 
            {
                var Headers = _httpContextAccessor.HttpContext!.Request.Headers;
                Console.WriteLine(Headers);
                var iconStream = await _httpClient.GetStreamAsync(uriIcon.Icon, ct);
                var fileStream = File.Create(filePath);
                await iconStream.CopyToAsync(fileStream, ct);
                fileStream.Close();
                return IconSuccess.Success(File.OpenRead(filePath));
            }
            catch(Exception ex)
            {
                _logger.LogError("Icon not found for uri {uri}, providing default.", uri, ex.Message, ex.StackTrace);
                return IconSuccess.Failure;
            }
        }
        return IconSuccess.Success(File.OpenRead(filePath));
    }

    
    private async Task EjectUriIcon(Uri uri, CancellationToken ct)
    {
        _uriCache.Remove(uri);
        await _uriIconAccessor.DeleteUriIcon(uri, ct);
    }
    private async Task<IUriIcon?> GetUriIcon(Uri uri, CancellationToken ct)
    {
        if (_uriCache.ContainsKey(uri) && _uriCache[uri].RetrievedDate >= DateTime.UtcNow.AddHours(-1))
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
        
        
        
        
        var response = await cli.GetAsync(uri);
        //If it's going strictly more or strictly less precise than the original uri, we'll use it.
        //If it's like "app.domain.com" to "login.domain.com", we don't use it.
        if (!response!.RequestMessage!.RequestUri!.Host.Contains(uri.Host) && !uri.Host.Contains(response.RequestMessage.RequestUri.Host))
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
        string favPath = favPathMatches.Any(m => m.Success) ? favPathMatches.First(m => m.Success).Groups[1].Value : "/favicon.ico";
	    return new Uri(response.RequestMessage!.RequestUri!, favPath);
    }
}

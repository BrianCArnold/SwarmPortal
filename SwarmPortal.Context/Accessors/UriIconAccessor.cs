using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

public class UriIconAccessor : IUriIconAccessor
{
    private ISourceContext _context;

    public UriIconAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task DeleteUriIcon(Uri uri, CancellationToken ct = default)
    {
        var cleanUri = CleanUri(uri);
        var uriIcon = await _context.UriIcons.SingleAsync(x => x.Uri == cleanUri, ct);
        _context.UriIcons.Remove(uriIcon);
        await _context.SaveChangesAsync(ct);
    }
    public async Task<bool> ContainsUriIcon(Uri uri, CancellationToken ct = default)
    {
        var cleanUri = CleanUri(uri);
        return await _context.UriIcons.AnyAsync(x => x.Uri == cleanUri, ct);
    }

    public async Task<IUriIcon> GetUriIcon(Uri uri, CancellationToken ct = default)
    {
        var cleanUri = CleanUri(uri);
        return await _context.UriIcons.SingleAsync(x => x.Uri == cleanUri, ct);
    }

    public async Task<IEnumerable<IUriIcon>> GetUriIcons(CancellationToken ct = default)
    {
        return await _context.UriIcons.ToListAsync(ct);
    }


    public async Task<IUriIcon> UpsertUriIcon(Uri uri, Uri icon, CancellationToken ct = default)
    {
        var cleanUri = CleanUri(uri);
        var uriIcon = await _context.UriIcons.AddOrUpdateAsync(
            x => x.Uri == cleanUri, 
            i => { i.Icon = icon; i.RetrievedDate = DateTime.UtcNow; }, 
            () => new UriIcon { Uri = cleanUri, Icon = icon, RetrievedDate = DateTime.UtcNow }, 
            ct);
        await _context.SaveChangesAsync(ct);
        return uriIcon;
    }

    private Uri CleanUri(Uri source)
    {
        return new Uri(source, "/");
    }
}

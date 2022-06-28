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
        var oldIconTask = _context.UriIcons.SingleOrDefaultAsync(x => x.Uri == cleanUri, ct);
        var uriIcon = new UriIcon()
        {
            Uri = cleanUri,
            Icon = icon,
            RetrievedDate = DateTime.UtcNow
        };
        var oldIcon = await oldIconTask;
        _context.UriIcons.Add(uriIcon);
        if (oldIcon is not null)
        {
            _context.UriIcons.Remove(oldIcon);
        }
        await _context.SaveChangesAsync(ct);
        return uriIcon;
    }

    private Uri CleanUri(Uri source)
    {
        return new Uri(source, "/");
    }
}

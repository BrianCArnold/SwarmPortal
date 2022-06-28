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
        var uriIcon = await _context.UriIcons.SingleAsync(x => x.Uri == uri, ct);
        _context.UriIcons.Remove(uriIcon);
        await _context.SaveChangesAsync(ct);
    }
    public async Task<bool> ContainsUriIcon(Uri uri, CancellationToken ct = default)
    {
        return await _context.UriIcons.AnyAsync(x => x.Uri == uri, ct);
    }

    public async Task<IUriIcon> GetUriIcon(Uri uri, CancellationToken ct = default)
    {
        return await _context.UriIcons.SingleAsync(x => x.Uri == uri, ct);
    }

    public async Task<IEnumerable<IUriIcon>> GetUriIcons(CancellationToken ct = default)
    {
        return await _context.UriIcons.ToListAsync(ct);
    }

    public async Task<IUriIcon> UpsertUriIcon(Uri uri, Uri icon, CancellationToken ct = default)
    {
        var oldIconTask = _context.UriIcons.SingleOrDefaultAsync(x => x.Uri == uri, ct);
        var uriIcon = new UriIcon()
        {
            Uri = uri,
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
}

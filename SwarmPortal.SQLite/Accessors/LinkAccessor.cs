using Microsoft.EntityFrameworkCore;
using SwarmPortal.Common;

namespace SwarmPortal.SQLite;

public class LinkAccessor : ILinkAccessor
{
    private ISourceContext _context;

    public LinkAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task AddLink(ILinkItem link, CancellationToken ct = default)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(g => g.Name == link.Group, ct) ?? new Group{ Name = link.Group };
        var allRoles = await _context.Roles.ToDictionaryAsync(r => r.Name, ct);
        var rolesForLink = link.Roles.Select(r => allRoles.ContainsKey(r) ? allRoles[r]: new Role{ Name = r }).ToList();
        var dbLink = new Link{
            Name = link.Name,
            Group = group,
            Url = link.Url,
            Roles = rolesForLink
        };
        _context.Links.Add(dbLink);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteLink(ILinkItem link, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Where(l => l.Name == link.Name && l.Group.Name == link.Group).SingleAsync(ct);
        _context.Links.Remove(dblink);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<string>> GetLinkGroups(CancellationToken ct = default)
    {
        return await _context.Groups.Select(g => g.Name).ToListAsync(ct);
    }

    public async Task<IEnumerable<ILinkItem>> GetLinks(CancellationToken ct = default)
    {
        return await _context.Links.Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url, l.Roles.Select(r => r.Name))).ToListAsync(ct);
    }

    public async Task<IEnumerable<ILinkItem>> GetLinksForGroup(string group, CancellationToken ct = default)
    {
        return await _context.Links.Where(l => l.Group.Name == group).Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url, l.Roles.Select(r => r.Name))).ToListAsync(ct);
    }

    public async Task<IEnumerable<ILinkItem>> GetLinksForGroupAndRole(string group, string role, CancellationToken ct = default)
    {
        return await _context.Links.Where(l => l.Group.Name == group && l.Roles.Any(r => r.Name == role)).Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url, l.Roles.Select(r => r.Name))).ToListAsync(ct);
    }

    public async Task<IEnumerable<ILinkItem>> GetLinksForRole(string role, CancellationToken ct = default)
    {
        return await _context.Links.Where(l => l.Roles.Any(r => r.Name == role)).Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url, l.Roles.Select(r => r.Name))).ToListAsync(ct);
    }
}
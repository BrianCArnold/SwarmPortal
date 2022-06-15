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
        var dbLink = new Link{
            Name = link.Name,
            Group = group,
            Url = link.Url
        };
        _context.Links.Add(dbLink);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteLink(string linkName, string linkGroup, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Where(l => l.Name == linkName && l.Group.Name == linkGroup).SingleAsync(ct);
        _context.Links.Remove(dblink);
        await _context.SaveChangesAsync(ct);
    }
    public async Task AddLinkRole(string linkName, string linkGroup, string linkRole, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Where(l => l.Name == linkName && l.Group.Name == linkGroup).SingleAsync(ct);
        var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == linkRole, ct) ?? new Role{ Name = linkRole };
        dblink.Roles.Add(role);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteLinkRole(string linkName, string linkGroup, string linkRole, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Where(l => l.Name == linkName && l.Group.Name == linkGroup).SingleAsync(ct);
        var dbRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == linkRole, ct);
        if (dbRole == null)
            return;
        dblink.Roles.Remove(dbRole);
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
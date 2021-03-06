using Microsoft.EntityFrameworkCore;
using SwarmPortal.Common;

namespace SwarmPortal.Context;

public class LinkAccessor : ILinkAccessor
{
    private ISourceContext _context;

    public LinkAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task<ILink> AddLink(string name, string group, string url, CancellationToken ct = default)
    {
        var link = await _context.Links.RemoveExtrasAsync(
            x => x.Name == name && x.Group.Name == group, 
            l => { l.Url = url; l.Enabled = true; }, 
            () => new Link { Name = name, Enabled = true, Group = _context.Groups.Single(g => g.Name == group), Url = url },
            ct);
        // var group = await _context.Groups.SingleOrDefaultAsync(g => g.Name == link.Group, ct) ?? new Group{ Name = link.Group };
        // var allRoles = await _context.Roles.ToDictionaryAsync(r => r.Name, ct);
        // var rolesForLink = link.Roles.Select(r => allRoles.ContainsKey(r) ? allRoles[r]: new Role{ Name = r }).ToList();
        // var dbLink = new Link{
        //     Name = link.Name,
        //     Group = group,
        //     Url = link.Url,
        //     Roles = rolesForLink
        // };
        // _context.Links.Add(dbLink);
        await _context.SaveChangesAsync(ct);
        return link;
    }

    public async Task DisableLink(ulong linkId, CancellationToken ct = default)
    {
        var dblink = await _context.Links.SingleAsync(l => l.Id == linkId, ct);
        dblink.Enabled = false;
        await _context.SaveChangesAsync(ct);
    }  
    public async Task EnableLink(ulong linkId, CancellationToken ct = default)
    {
        var dblink = await _context.Links.SingleAsync(l => l.Id == linkId, ct);
        dblink.Enabled = true;
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddLinkRole(ulong linkId, string linkRole, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Include(l => l.Roles).SingleAsync(l => l.Id == linkId, ct);
        var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == linkRole, ct) ?? new Role{ Name = linkRole };
        dblink.Roles.Add(role);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteLinkRole(ulong linkId, string linkRole, CancellationToken ct = default)
    {
        var dblink = await _context.Links.Include(l => l.Roles).SingleAsync(l => l.Id == linkId, ct);
        var dbRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == linkRole, ct);
        if (dbRole == null)
            return;
        dblink.Roles.Remove(dbRole);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<ILink>> GetLinks(CancellationToken ct = default)
    {
        return await _context.Links
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .ToListAsync(ct);
    }
    public async Task<IEnumerable<ILink>> GetEnabledLinks(CancellationToken ct = default)
    {
        return await _context.Links
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .Where(l => l.Enabled == true)
            .ToListAsync(ct);
    }
    public async Task<IEnumerable<ILink>> GetDisabledLinks(CancellationToken ct = default)
    {
        return await _context.Links
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .Where(l => l.Enabled == false)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ILink>> GetLinksForGroup(string group, CancellationToken ct = default)
    {
        return await _context.Links
            .Where(l => l.Group.Name == group)
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ILink>> GetLinksForGroupAndRole(string group, string role, CancellationToken ct = default)
    {
        return await _context.Links
            .Where(l => l.Group.Name == group && l.Roles.Any(r => r.Name == role))
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ILink>> GetLinksForRole(string role, CancellationToken ct = default)
    {
        return await _context.Links
            .Where(l => l.Roles.Any(r => r.Name == role))
            .Include(l => l.Group)
            .Include(l => l.Roles)
            .ToListAsync(ct);
    }
}
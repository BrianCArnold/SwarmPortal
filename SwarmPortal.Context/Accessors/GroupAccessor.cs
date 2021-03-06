using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

public class GroupAccessor : IGroupAccessor
{
    private ISourceContext _context;

    public GroupAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task<IGroup> AddGroup(string groupName, CancellationToken ct = default)
    {
        var group = await _context.Groups.RemoveExtrasAsync(
            x => x.Name == groupName, 
            l => { l.Enabled = true; }, 
            () => new Group{ Name = groupName },
            ct);
        await _context.SaveChangesAsync(ct);
        return group;
    }

    public async Task DisableGroup(ulong groupId, CancellationToken ct = default)
    {
        if (await _context.Links.AnyAsync(l => l.Group.Id == groupId, ct))
        {
            throw new InvalidOperationException("Group has links, please delete them first");
        }
        else 
        {
            var dbGroup = await _context.Groups.SingleAsync(r => r.Id == groupId, ct);
            dbGroup.Enabled = false;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task EnableGroup(ulong groupId, CancellationToken ct = default)
    {
        var dbGroup = await _context.Groups.SingleAsync(r => r.Id == groupId, ct);
        dbGroup.Enabled = true;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<IGroup>> GetGroups(CancellationToken ct = default)
    {
        return await _context.Groups.ToListAsync(ct);
    }
    public async Task<IEnumerable<IGroup>> GetEnabledGroupsWithNoLinks(CancellationToken ct = default)
    {
        return await _context.Groups.Where(r => !r.Links.Any() && r.Enabled).ToListAsync(ct);
    }
    public async Task<IEnumerable<IGroup>> GetDisabledGroups(CancellationToken ct = default)
    {
        return await _context.Groups.Where(r => !r.Enabled).ToListAsync(ct);
    }

}
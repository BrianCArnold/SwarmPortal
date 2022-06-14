using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.SQLite;

public class GroupAccessor : IGroupAccessor
{
    private ISourceContext _context;

    public GroupAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task AddGroup(string groupName, CancellationToken ct = default)
    {
        if (await _context.Groups.AnyAsync(r => r.Name == groupName, ct))
        {
            throw new InvalidOperationException("Group already exists");
        }
        else {
            _context.Groups.Add(new Group{ Name = groupName });
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteGroup(string groupName, CancellationToken ct = default)
    {
        if (await _context.Links.AnyAsync(l => l.Group.Name == groupName, ct))
        {
            throw new InvalidOperationException("Group has links, please delete them first");
        }
        else 
        {
            var dbGroup = await _context.Groups.SingleOrDefaultAsync(r => r.Name == groupName, ct);
            if (dbGroup != null)
            {
                _context.Groups.Remove(dbGroup);
                await _context.SaveChangesAsync(ct);
            }
        }
    }

    public async Task<IEnumerable<string>> GetGroups(CancellationToken ct = default)
    {
        return await _context.Groups.Select(r => r.Name).ToListAsync(ct);
    }

}
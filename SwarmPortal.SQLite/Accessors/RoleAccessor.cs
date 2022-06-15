using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.SQLite;

public class RoleAccessor : IRoleAccessor
{
    private ISourceContext _context;

    public RoleAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task AddRole(string role, CancellationToken ct = default)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == role, ct))
        {
            throw new InvalidOperationException("Role already exists");
        }
        else {
            _context.Roles.Add(new Role{ Name = role });
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteRole(ulong roleId, CancellationToken ct = default)
    {
        if (await _context.Links.AnyAsync(l => l.Roles.Any(r => r.Id == roleId), ct))
        {
            throw new InvalidOperationException("Role has links, please delete them first");
        }
        else 
        {
            var dbRole = await _context.Roles.SingleOrDefaultAsync(r => r.Id == roleId, ct);
            if (dbRole != null)
            {
                _context.Roles.Remove(dbRole);
                await _context.SaveChangesAsync(ct);
            }
        }
    }

    public async Task<IEnumerable<IRole>> GetRoles(CancellationToken ct = default)
    {
        return await _context.Roles.ToListAsync(ct);
    }



}
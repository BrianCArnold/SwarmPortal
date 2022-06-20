using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.SQLite;

public class RoleAccessor : IRoleAccessor
{
    private ISourceContext _context;

    public RoleAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task<IRole> AddRole(string role, CancellationToken ct = default)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == role, ct))
        {
            throw new InvalidOperationException("Role already exists");
        }
        else {
            var roleObj = new Role{ Name = role };
            _context.Roles.Add(roleObj);
            await _context.SaveChangesAsync(ct);
            return roleObj;
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

    public async Task DisableRole(ulong roleId, CancellationToken ct = default)
    {
        if (await _context.Links.AnyAsync(l => l.Roles.Any(r => r.Id == roleId), ct))
        {
            throw new InvalidOperationException("Role has links, please delete them first");
        }
        else 
        {
            var role = await _context.Roles.SingleAsync(r => r.Id == roleId);
            role.Enabled = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task EnableRole(ulong roleId, CancellationToken ct = default)
    {
        var role = await _context.Roles.SingleAsync(r => r.Id == roleId);
        role.Enabled = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<IRole>> GetRoles(CancellationToken ct = default)
    {
        return await _context.Roles.ToListAsync(ct);
    }
    public async Task<IEnumerable<IRole>> GetEnabledRolesWithNoLinks(CancellationToken ct = default)
    {
        return await _context.Roles.Where(r => !r.Links.Any() && r.Enabled).ToListAsync(ct);
    }
    public async Task<IEnumerable<IRole>> GetDisabledRoles(CancellationToken ct = default)
    {
        return await _context.Roles.Where(r => !r.Enabled).ToListAsync(ct);
    }

}
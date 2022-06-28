using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

public class RoleAccessor : IRoleAccessor
{
    private ISourceContext _context;

    public RoleAccessor(ISourceContext context)
    {
        _context = context;
    }
    public async Task<IRole> AddRole(string role, CancellationToken ct = default)
    {
        var outRole = await _context.Roles.RemoveExtrasAsync(
            x => x.Name == role, 
            r => r.Enabled = true, 
            () => new Role { Name = role, Enabled = true },
            ct);
        await _context.SaveChangesAsync(ct);
        return outRole;
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
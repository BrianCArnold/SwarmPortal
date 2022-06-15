namespace SwarmPortal.SQLite;
public interface IRoleAccessor
{
    Task<IEnumerable<IRole>> GetRoles(CancellationToken ct = default);
    Task AddRole(string role, CancellationToken ct = default);
    Task DeleteRole(ulong roleId, CancellationToken ct = default);
}

namespace SwarmPortal.SQLite;
public interface IRoleAccessor
{
    Task<IEnumerable<string>> GetRoles(CancellationToken ct = default);
    Task AddRole(string role, CancellationToken ct = default);
    Task DeleteRole(string role, CancellationToken ct = default);
}

namespace SwarmPortal.Context;
public interface IRoleAccessor
{
    Task<IEnumerable<IRole>> GetRoles(CancellationToken ct = default);
    Task<IEnumerable<IRole>> GetEnabledRolesWithNoLinks(CancellationToken ct = default);
    Task<IEnumerable<IRole>> GetDisabledRoles(CancellationToken ct = default);
    Task<IRole> AddRole(string role, CancellationToken ct = default);
    Task EnableRole(ulong roleId, CancellationToken ct = default);
    Task DisableRole(ulong roleId, CancellationToken ct = default);
}

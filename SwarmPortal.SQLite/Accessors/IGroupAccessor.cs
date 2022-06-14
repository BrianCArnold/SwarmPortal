namespace SwarmPortal.SQLite;

public interface IGroupAccessor
{
    Task<IEnumerable<string>> GetGroups(CancellationToken ct = default);
    Task AddGroup(string role, CancellationToken ct = default);
    Task DeleteGroup(string role, CancellationToken ct = default);
}

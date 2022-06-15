namespace SwarmPortal.SQLite;

public interface IGroupAccessor
{
    Task<IEnumerable<IGroup>> GetGroups(CancellationToken ct = default);
    Task AddGroup(string groupName, CancellationToken ct = default);
    Task DeleteGroup(ulong groupId, CancellationToken ct = default);
}

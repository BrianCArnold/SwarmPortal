namespace SwarmPortal.Context;

public interface IGroupAccessor
{
    Task<IEnumerable<IGroup>> GetGroups(CancellationToken ct = default);
    Task<IEnumerable<IGroup>> GetEnabledGroupsWithNoLinks(CancellationToken ct = default);
    Task<IEnumerable<IGroup>> GetDisabledGroups(CancellationToken ct = default);
    Task<IGroup> AddGroup(string groupName, CancellationToken ct = default);
    Task DisableGroup(ulong groupId, CancellationToken ct = default);
    Task EnableGroup(ulong groupId, CancellationToken ct = default);
}

using SwarmPortal.Common;

namespace SwarmPortal.SQLite;

public interface ILinkAccessor
{
    Task<IEnumerable<ILinkItem>> GetLinks(CancellationToken ct = default);
    Task<IEnumerable<string>> GetLinkGroups(CancellationToken ct = default);
    Task<IEnumerable<ILinkItem>> GetLinksForRole(string role, CancellationToken ct = default);
    Task<IEnumerable<ILinkItem>> GetLinksForGroup(string group, CancellationToken ct = default);
    Task<IEnumerable<ILinkItem>> GetLinksForGroupAndRole(string group, string role, CancellationToken ct = default);
    Task AddLink(ILinkItem link, CancellationToken ct = default);
    Task DeleteLink(string linkName, string linkGroup, CancellationToken ct = default);
    Task AddLinkRole(string linkName, string linkGroup, string linkRole, CancellationToken ct = default);
    Task DeleteLinkRole(string linkName, string linkGroup, string linkRole, CancellationToken ct = default);
}

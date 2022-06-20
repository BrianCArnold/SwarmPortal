using SwarmPortal.Common;

namespace SwarmPortal.SQLite;

public interface ILinkAccessor
{
    Task<IEnumerable<ILink>> GetLinks(CancellationToken ct = default);
    Task<IEnumerable<ILink>> GetLinksForRole(string role, CancellationToken ct = default);
    Task<IEnumerable<ILink>> GetLinksForGroup(string group, CancellationToken ct = default);
    Task<IEnumerable<ILink>> GetLinksForGroupAndRole(string group, string role, CancellationToken ct = default);
    Task<ILink> AddLink(ILinkItem link, CancellationToken ct = default);
    Task DisableLink(ulong linkId, CancellationToken ct = default);
    Task EnableLink(ulong linkId, CancellationToken ct = default);
    Task AddLinkRole(ulong linkId, string linkRole, CancellationToken ct = default);
    Task DeleteLinkRole(ulong linkId, string linkRole, CancellationToken ct = default);
}

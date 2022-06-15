
using SwarmPortal.Common;

namespace SwarmPortal.SQLite;

public interface ILink
{
    ulong Id { get; }
    string Name { get; }
    string Url { get; }
    Group Group { get; }
    IEnumerable<IRole> Roles { get; }
}


using SwarmPortal.Common;

namespace SwarmPortal.Context;

public interface ILink
{
    ulong Id { get; }
    string Name { get; }
    string Url { get; }
    Group Group { get; }
    bool Enabled { get; }
    IEnumerable<IRole> Roles { get; }
}

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

[Index(nameof(Name), IsUnique = true)]
public class Link : ILink
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public string Name { get; set; } = null!;
    [NotNull]
    public string Url { get; set; } = null!;
    public Group Group { get; set; } = null!;
    public bool Enabled { get; set; }
    public ICollection<Role> Roles { get; set; } = null!;
    IEnumerable<IRole> ILink.Roles  => Roles;
}

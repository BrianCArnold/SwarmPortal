using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

[Index(nameof(Name), IsUnique = true)]
public class Role : IRole
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public string Name { get; set; } = null!;
    [NotNull]
    public bool Enabled { get; set; }
    public ICollection<Link> Links { get; set; } = null!;
}
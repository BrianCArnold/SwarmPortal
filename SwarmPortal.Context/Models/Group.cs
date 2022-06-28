using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace SwarmPortal.Context;

[Index(nameof(Name), IsUnique = true)]
public class Group : IGroup
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public string Name { get; set; } = null!;
    public bool Enabled { get; set; }
    [JsonIgnore] 
    public ICollection<Link> Links { get; set; } = null!;
}

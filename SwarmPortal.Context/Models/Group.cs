
using Newtonsoft.Json;

namespace SwarmPortal.Context;

public class Group : IGroup
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Enabled { get; set; }
    [JsonIgnore] 
    public ICollection<Link> Links { get; set; } = null!;
}

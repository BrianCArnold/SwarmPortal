
using Newtonsoft.Json;

namespace SwarmPortal.SQLite;

public class Group : IGroup
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    [JsonIgnore] 
    public ICollection<Link> Links { get; set; } = null!;
}

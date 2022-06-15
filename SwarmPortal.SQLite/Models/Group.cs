
using Newtonsoft.Json;

namespace SwarmPortal.SQLite;

public class Group : IGroup
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore] 
    public ICollection<Link> Links { get; set; }
}

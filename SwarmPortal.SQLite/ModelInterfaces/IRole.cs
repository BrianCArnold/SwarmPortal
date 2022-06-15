using Newtonsoft.Json;

namespace SwarmPortal.SQLite;

public interface IRole
{
    ulong Id { get; set; }
    string Name { get; set; }
    [JsonIgnore] 
    ICollection<Link> Links { get; set; }
}

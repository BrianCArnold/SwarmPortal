using Newtonsoft.Json;

namespace SwarmPortal.Context;

public interface IRole
{
    ulong Id { get; set; }
    string Name { get; set; }
    bool Enabled { get; set; }
    [JsonIgnore] 
    ICollection<Link> Links { get; set; }
}

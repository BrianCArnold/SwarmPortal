namespace SwarmPortal.SQLite;

public class Role : IRole
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public ICollection<Link> Links { get; set; }
}
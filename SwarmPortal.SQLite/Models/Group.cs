namespace SwarmPortal.SQLite;

public class Group
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public ICollection<Link> Links { get; set; }   
}

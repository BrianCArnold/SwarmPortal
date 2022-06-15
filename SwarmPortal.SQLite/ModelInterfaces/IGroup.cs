namespace SwarmPortal.SQLite;

public interface IGroup
{
    ulong Id { get; set; }
    string Name { get; set; }
    ICollection<Link> Links { get; set; }
}

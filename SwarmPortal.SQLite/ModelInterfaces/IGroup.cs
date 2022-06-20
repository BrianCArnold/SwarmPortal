namespace SwarmPortal.SQLite;

public interface IGroup
{
    ulong Id { get; set; }
    string Name { get; set; }
    bool Enabled { get; }
    ICollection<Link> Links { get; set; }
}

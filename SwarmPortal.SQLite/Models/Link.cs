namespace SwarmPortal.SQLite;

public class Link : ILink
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Group Group { get; set; }
    public ICollection<Role> Roles { get; set; }
    IEnumerable<IRole> ILink.Roles  => Roles;
}

namespace SwarmPortal.SQLite;

public class Link : ILink
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Group Group { get; set; } = null!;
    public ICollection<Role> Roles { get; set; } = null!;
    IEnumerable<IRole> ILink.Roles  => Roles;
}

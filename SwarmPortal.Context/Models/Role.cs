namespace SwarmPortal.Context;

public class Role : IRole
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Enabled { get; set; }
    public ICollection<Link> Links { get; set; } = null!;
}
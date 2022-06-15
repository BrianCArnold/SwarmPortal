
namespace SwarmPortal.SQLite;

public class SwarmPortalUser
{
    public ulong Id { get; set; }
    public string OIDCUserKey { get; set; }
    public ICollection<Link> Links { get; set; }
}


namespace SwarmPortal.Context;

public class SwarmPortalUser
{
    public ulong Id { get; set; }
    public string OIDCUserKey { get; set; } = null!;
    public ICollection<Link> Links { get; set; } = null!;
}

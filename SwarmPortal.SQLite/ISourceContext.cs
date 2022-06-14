using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.SQLite
{
    public interface ISourceContext
    {
        DbSet<SwarmPortalUser> Users { get; }
        DbSet<Group> Groups { get; }
        DbSet<Link> Links { get; }
        DbSet<Role> Roles { get; }
    }
}
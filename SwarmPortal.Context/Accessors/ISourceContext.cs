using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context
{
    public interface ISourceContext
    {
        DbSet<SwarmPortalUser> Users { get; }
        DbSet<Group> Groups { get; }
        DbSet<Link> Links { get; }
        DbSet<Role> Roles { get; }
        DbSet<UriIcon> UriIcons { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
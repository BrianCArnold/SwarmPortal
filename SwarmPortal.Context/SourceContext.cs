using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

public class SourceContext : DbContext, ISourceContext
{
    public DbSet<SwarmPortalUser> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Link> Links { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UriIcon> Icons { get; set; } = null!;


    public SourceContext(DbContextOptions<SourceContext> options): base(options) {
        Database.MigrateAsync().Wait();
    }

}

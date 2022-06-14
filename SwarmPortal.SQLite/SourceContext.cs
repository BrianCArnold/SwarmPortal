using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SwarmPortal.Source;

namespace SwarmPortal.SQLite;

public class SourceContext : DbContext, ISourceContext
{
    private readonly ISQLiteSourceConfiguration config;
    
    public DbSet<SwarmPortalUser> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Link> Links { get; set; }


    public SourceContext(DbContextOptions<SourceContext> options): base(options) {
        Database.MigrateAsync().Wait();
    }
    

}

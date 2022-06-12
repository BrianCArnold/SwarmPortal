using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Source.SQLite;

public class SourceContext : DbContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Link> Links { get; set; }
}

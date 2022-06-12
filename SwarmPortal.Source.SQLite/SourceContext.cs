using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Source.SQLite;

public class SourceContext : DbContext
{
    private readonly ISQLiteSourceConfiguration config;

    public DbSet<Group> Groups { get; set; }
    public DbSet<Link> Links { get; set; }

    private string DbPath { get; }

    public SourceContext(ISQLiteSourceConfiguration config)
    {
        this.config = config;

        var path = Path.Join(Environment.CurrentDirectory, config.SQLiteFileDirectory);
        DbPath = System.IO.Path.Join(path, "swarmportal.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

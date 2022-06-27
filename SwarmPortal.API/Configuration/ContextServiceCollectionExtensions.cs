using Microsoft.EntityFrameworkCore;
using SwarmPortal.Common;
using SwarmPortal.Context;

namespace SwarmPortal.API;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddSwarmPortalContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<SourceContext>(o =>
        {
            var databaseType = config.GetValue<string>("DatabaseType");
            switch (databaseType)
            {
                case "SQLite":
                    o.UseSqlite(config.GetConnectionString("SQLite"), b => b.MigrationsAssembly("SwarmPortal.SQLiteMigrations"));
                    break;
                case "SQLServer":
                    o.UseSqlServer(config.GetConnectionString("SQLServer"), b => b.MigrationsAssembly("SwarmPortal.SqlServerMigrations"));
                    break;
                default:
                    Console.Error.WriteAsync("ERROR: No database type specified in configuration file.");
                    throw new Exception("No database type specified in configuration file.");
            }
        });
        services.AddScoped<ISourceContext>(services => services.GetRequiredService<SourceContext>());
        return services;
    }
    // public static IServiceCollection AddDefaultIdentity(this IServiceCollection services)
    //  {
    //     services.AddIdentity<SwarmPortal.SQLite.SourceUser, SwarmPortal.SQLite.SourceRole>();
    //     return services;
    //  }
    public static IServiceCollection AddSQLiteLinkProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, SqlLinkItemProvider>();
    public static IServiceCollection AddSQLiteAccessors(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IGroupAccessor, GroupAccessor>()
                         .AddScoped<ILinkAccessor, LinkAccessor>()
                         .AddScoped<IRoleAccessor, RoleAccessor>()
                         .AddScoped<IUriIconAccessor, UriIconAccessor>();
}
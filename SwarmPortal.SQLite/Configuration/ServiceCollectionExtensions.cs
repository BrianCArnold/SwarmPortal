using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Source;
using Microsoft.Extensions.Configuration;
using SwarmPortal.SQLite;
using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddSQLiteFileConfiguration(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<ISQLiteSourceConfiguration>(services => SQLiteSourceConfiguration.Create(services.GetRequiredService<IConfiguration>()));
    public static IServiceCollection AddSwarmPortalSQLiteContext(this IServiceCollection services, IConfiguration config) 
    {
        var sqlFileDirectory = config["SQLiteFileDirectory"] ?? "persist";
        var path = Path.Join(Environment.CurrentDirectory, sqlFileDirectory);
        var dbPath = Path.Join(path, "swarmportal.db");
        var connectionString = $"DataSource={dbPath};Cache=Shared";
        services.AddDbContext<SourceContext>(o => o.UseSqlite(connectionString));
        services.AddScoped<ISourceContext>(services => services.GetRequiredService<SourceContext>());
        return services;
    }
    // public static IServiceCollection AddDefaultIdentity(this IServiceCollection services)
    //  {
    //     services.AddIdentity<SwarmPortal.SQLite.SourceUser, SwarmPortal.SQLite.SourceRole>();
    //     return services;
    //  }
    public static IServiceCollection AddSQLiteLinkProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, SQLiteFileLinkItemProvider>();
    public static IServiceCollection AddSQLiteAccessors(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IGroupAccessor, GroupAccessor>()
                         .AddScoped<ILinkAccessor, LinkAccessor>()
                         .AddScoped<IRoleAccessor, RoleAccessor>();
}
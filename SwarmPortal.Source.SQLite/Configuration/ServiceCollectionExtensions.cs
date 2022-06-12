using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Source;
using Microsoft.Extensions.Configuration;
using SwarmPortal.Source.SQLite;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddSQLiteFileConfiguration(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<ISQLiteSourceConfiguration>(services => SQLiteSourceConfiguration.Create(services.GetRequiredService<IConfiguration>()));
    public static IServiceCollection AddSQLiteContext(this IServiceCollection services) 
     => services.AddDbContext<SwarmPortal.Source.SQLite.SourceContext>();
    // public static IServiceCollection AddDefaultIdentity(this IServiceCollection services)
    //  {
    //     services.AddIdentity<SwarmPortal.Source.SQLite.SourceUser, SwarmPortal.Source.SQLite.SourceRole>();
    //     return services;
    //  }
    public static IServiceCollection AddSQLiteLinkProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, SQLiteFileLinkItemProvider>();
}
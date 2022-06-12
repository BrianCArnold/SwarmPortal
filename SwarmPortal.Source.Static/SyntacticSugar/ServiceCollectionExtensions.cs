using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Static;
using SwarmPortal.Source;
using Microsoft.Extensions.Configuration;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddStaticFileConfiguration(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IStaticSourceConfiguration>(services => StaticSourceConfiguration.Create(services.GetRequiredService<IConfiguration>()));
    public static IServiceCollection AddStaticLinkFileProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, StaticFileLinkItemProvider>();
}
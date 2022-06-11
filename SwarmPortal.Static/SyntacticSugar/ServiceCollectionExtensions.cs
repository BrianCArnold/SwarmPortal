using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Static;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddStaticHostStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IGroupableHostItem>, StaticHostItemProvider>();
    public static IServiceCollection AddStaticLinkStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IGroupableLinkItem>, StaticLinkItemProvider>();
}
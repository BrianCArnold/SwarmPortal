using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Static;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddStaticHostStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IHostItem>, StaticHostItemProvider>();
    public static IServiceCollection AddStaticLinkStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, StaticLinkItemProvider>();
}
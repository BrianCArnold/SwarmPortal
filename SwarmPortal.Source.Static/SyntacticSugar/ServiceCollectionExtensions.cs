using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Static;

namespace SwarmPortal.Common;
public static class SwarmPortalStaticServiceExtensions
{
    public static IServiceCollection AddStaticStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IStatusItem>, StaticStatusItemProvider>();
    public static IServiceCollection AddStaticLinkProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, StaticLinkItemProvider>();
}
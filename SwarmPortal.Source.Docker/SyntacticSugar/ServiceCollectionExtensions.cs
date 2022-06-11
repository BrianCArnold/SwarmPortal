using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.Source.Docker;

namespace SwarmPortal.Common;
public static class SwarmPortalDockerServiceExtensions
{
    public static IServiceCollection AddDockerServiceStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IStatusItem>, DockerSwarmServiceStatusItemProvider>();
    public static IServiceCollection AddDockerNodeStatusProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<IStatusItem>, DockerSwarmNodeStatusItemProvider>();
    public static IServiceCollection AddDockerServiceLinkProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemProvider<ILinkItem>, DockerSwarmServiceLinkItemProvider>();
}
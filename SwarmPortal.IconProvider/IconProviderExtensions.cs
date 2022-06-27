namespace SwarmPortal.Common;

using Microsoft.Extensions.DependencyInjection;
using SwarmPortal.IconProvider;

public static class IconProviderExtensions 
{
    public static IServiceCollection AddIconProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IIconProvider, IconProvider>();
}

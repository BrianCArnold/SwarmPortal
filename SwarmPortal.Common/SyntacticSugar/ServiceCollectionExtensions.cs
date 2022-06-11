using Microsoft.Extensions.DependencyInjection;

namespace SwarmPortal.Common;
public static class SwarmPortalCommonServiceExtensions
{
    public static IServiceCollection AddHostGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<IHostItem>, ItemDictionaryGeneratorProvider<IHostItem>>();
    public static IServiceCollection AddLinkGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<ILinkItem>, ItemDictionaryGeneratorProvider<ILinkItem>>();
}
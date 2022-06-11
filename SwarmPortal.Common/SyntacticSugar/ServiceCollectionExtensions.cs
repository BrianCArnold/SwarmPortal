using Microsoft.Extensions.DependencyInjection;

namespace SwarmPortal.Common;
public static class SwarmPortalCommonServiceExtensions
{
    public static IServiceCollection AddHostGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<IGroupableHostItem, IHostItem>, ItemDictionaryGeneratorProvider<IGroupableHostItem, IHostItem>>();
    public static IServiceCollection AddLinkGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<IGroupableLinkItem, ILinkItem>, ItemDictionaryGeneratorProvider<IGroupableLinkItem, ILinkItem>>();
}
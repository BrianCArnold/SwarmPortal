using Microsoft.Extensions.DependencyInjection;

namespace SwarmPortal.Common;
public static class SwarmPortalCommonServiceExtensions
{
    public static IServiceCollection AddStatusGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<IStatusItem>, ItemDictionaryGeneratorProvider<IStatusItem>>();
    public static IServiceCollection AddLinkGroupCoalescerProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryGeneratorProvider<ILinkItem>, ItemDictionaryGeneratorProvider<ILinkItem>>();
}
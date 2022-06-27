using Microsoft.Extensions.DependencyInjection;

namespace SwarmPortal.Common;
public static class SwarmPortalCommonServiceExtensions
{
    public static IServiceCollection AddStatusGroupDictionaryProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryProvider<IStatusItem>, ItemDictionaryProvider<IStatusItem>>();
    public static IServiceCollection AddLinkGroupDictionaryProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemDictionaryProvider<ILinkItem>, ItemDictionaryProvider<ILinkItem>>();


    public static IServiceCollection AddStatusCoalescedItemProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<ICoalescedItemProvider<IStatusItem>, CoalescedItemProvider<IStatusItem>>();
    public static IServiceCollection AddLinkCoalescedItemProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<ICoalescedItemProvider<ILinkItem>, CoalescedItemProvider<ILinkItem>>();


    public static IServiceCollection AddStatusOrderingProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemOrderingProvider<IStatusItem>, ItemOrderingProvider<IStatusItem>>();
    public static IServiceCollection AddLinkOrderingProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemOrderingProvider<ILinkItem>, ItemOrderingProvider<ILinkItem>>();


    public static IServiceCollection AddStatusRoleFilteringProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemRoleFilteringProvider<IStatusItem>, ItemRoleFilteringProvider<IStatusItem>>();
    public static IServiceCollection AddLinkRoleFilteringProvider(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IItemRoleFilteringProvider<ILinkItem>, ItemRoleFilteringProvider<ILinkItem>>();
}
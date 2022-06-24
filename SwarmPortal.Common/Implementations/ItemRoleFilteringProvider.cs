namespace SwarmPortal.Common;

internal class ItemRoleFilteringProvider<TRoleHavingItem> : IItemRoleFilteringProvider<TRoleHavingItem> 
    where TRoleHavingItem : IHasRoles
{
    public IAsyncEnumerable<TRoleHavingItem> FilterItemsByRoles(IAsyncEnumerable<TRoleHavingItem> items, IEnumerable<string> roles, CancellationToken ct)
    {
        return items.WhereAwait(item => ValueTask.FromResult(!item.Roles.Any() || item.Roles.Any(role => roles.Contains(role))));
    }
}

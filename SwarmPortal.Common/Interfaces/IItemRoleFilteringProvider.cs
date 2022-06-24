namespace SwarmPortal.Common;

public interface IItemRoleFilteringProvider<TRoleHavingItem> where TRoleHavingItem : IHasRoles
{
    IAsyncEnumerable<TRoleHavingItem> FilterItemsByRoles(IAsyncEnumerable<TRoleHavingItem> items, IEnumerable<string> roles, CancellationToken ct);
}

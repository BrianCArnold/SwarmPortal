namespace SwarmPortal.Common;

public interface ICoalescedItemProvider<TItem> where TItem : INamedItem, IGroupableItem, IHasRoles
{
    IAsyncEnumerable<TItem> GetItems(CancellationToken ct);
}

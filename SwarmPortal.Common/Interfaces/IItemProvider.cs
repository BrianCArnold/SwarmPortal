namespace SwarmPortal.Common;

public interface IItemProvider<TItem>
    where TItem : IGroupableItem
{
    IAsyncEnumerable<TItem> GetItemsAsync(CancellationToken ct);
}

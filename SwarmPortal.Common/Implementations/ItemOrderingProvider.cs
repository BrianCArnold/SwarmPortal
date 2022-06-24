namespace SwarmPortal.Common;

internal class ItemOrderingProvider<TOrderableItem> : IItemOrderingProvider<TOrderableItem> where TOrderableItem : INamedItem
{
    public IAsyncEnumerable<TOrderableItem> OrderItems(IAsyncEnumerable<TOrderableItem> items)
    {
        return items.OrderByAwait<TOrderableItem, string>(item => ValueTask.FromResult(item.Name));
    }
}

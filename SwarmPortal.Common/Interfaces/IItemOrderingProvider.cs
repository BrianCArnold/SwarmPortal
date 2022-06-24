namespace SwarmPortal.Common;

public interface IItemOrderingProvider<TOrderableItem> where TOrderableItem : INamedItem
{
    IAsyncEnumerable<TOrderableItem> OrderItems(IAsyncEnumerable<TOrderableItem> items);
}

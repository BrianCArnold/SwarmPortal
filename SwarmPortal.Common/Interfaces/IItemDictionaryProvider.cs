namespace SwarmPortal.Common;

public interface IItemDictionaryProvider<TGroupableItem>
    where TGroupableItem : class, IGroupableItem
{
    Task<Dictionary<string, IEnumerable<TGroupableItem>>> GetDictionaryAsync(IAsyncEnumerable<TGroupableItem> sources, CancellationToken ct);
}

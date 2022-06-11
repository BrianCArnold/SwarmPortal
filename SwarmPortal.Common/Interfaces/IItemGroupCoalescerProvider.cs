namespace SwarmPortal.Common;

public interface IItemDictionaryGeneratorProvider<TGroupableItem, TItem>
    where TItem : class, INamedItem
    where TGroupableItem : class, IGroupableItem
{
    DictionaryGenerator<TItem> GetDictionaryGeneratorAsync(CancellationToken ct);
}

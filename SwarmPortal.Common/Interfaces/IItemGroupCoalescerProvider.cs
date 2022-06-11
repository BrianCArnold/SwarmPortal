namespace SwarmPortal.Common;

public interface IItemDictionaryGeneratorProvider<TGroupableItem>
    where TGroupableItem : class, IGroupableItem, INamedItem
{
    DictionaryGenerator<TGroupableItem> GetDictionaryGeneratorAsync(CancellationToken ct);
}

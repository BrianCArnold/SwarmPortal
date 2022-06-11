namespace SwarmPortal.Common;

public class ItemDictionaryGeneratorProvider<TGroupableItem, TItem> : IItemDictionaryGeneratorProvider<TGroupableItem, TItem>
    where TItem : class, INamedItem
    where TGroupableItem : class, IGroupableItem
{
    private readonly IEnumerable<IItemProvider<TGroupableItem>> sources;

    public ItemDictionaryGeneratorProvider(IEnumerable<IItemProvider<TGroupableItem>> sources)
    {
        this.sources = sources;
    }
    public DictionaryGenerator<TItem> GetDictionaryGeneratorAsync(CancellationToken ct)
        => sources.ToAsyncEnumerable()
            .SelectMany(p => p.GetItemsAsync(ct))
            .GroupBy(i => i.Group, i => i as TItem)
            .ToDictionaryGenerator();
}
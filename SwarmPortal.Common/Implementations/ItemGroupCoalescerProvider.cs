namespace SwarmPortal.Common;

public class ItemDictionaryGeneratorProvider<TGroupableItem> : IItemDictionaryGeneratorProvider<TGroupableItem>
    where TGroupableItem : class, IGroupableItem, INamedItem
{
    private readonly IEnumerable<IItemProvider<TGroupableItem>> sources;

    public ItemDictionaryGeneratorProvider(IEnumerable<IItemProvider<TGroupableItem>> sources)
    {
        this.sources = sources;
    }
    public DictionaryGenerator<TGroupableItem> GetDictionaryGeneratorAsync(CancellationToken ct)
        => sources.ToAsyncEnumerable()
            .SelectMany(p => p.GetItemsAsync(ct))
            .OrderBy(i => i.Name)
            .GroupBy(i => i.Group)
            .OrderBy(g => g.Key)
            .ToDictionaryGenerator();
}
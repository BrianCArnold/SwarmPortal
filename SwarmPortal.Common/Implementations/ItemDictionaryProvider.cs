namespace SwarmPortal.Common;

internal class ItemDictionaryProvider<TGroupableItem> : IItemDictionaryProvider<TGroupableItem>
    where TGroupableItem : class, IGroupableItem
{
    public async Task<Dictionary<string, IEnumerable<TGroupableItem>>> GetDictionaryAsync(IAsyncEnumerable<TGroupableItem> sources, CancellationToken ct)
    {
        var groups = sources.GroupByAwait<TGroupableItem, string>(g => ValueTask.FromResult(g.Group));
        Dictionary<string, IEnumerable<TGroupableItem>> result = new Dictionary<string, IEnumerable<TGroupableItem>>();
        await foreach (var group in groups)
        {
            
            if (await group.AnyAsync())
            {
                result.Add(group.Key, await group.ToListAsync());
            }
        }
        return result;
    }
}
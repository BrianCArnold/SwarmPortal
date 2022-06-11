using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SwarmPortal.Common;

public class DictionaryGenerator<TGroupedItem>
    where TGroupedItem: INamedItem
{
    private readonly IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>> wrapped;

    public DictionaryGenerator(IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>> wrapped)
    {
        this.wrapped = wrapped;
    }
    public async Task<Dictionary<string, IEnumerable<TGroupedItem>>> GetDictionary(CancellationToken ct)
    {
        Dictionary<string, IEnumerable<TGroupedItem>> result = new Dictionary<string, IEnumerable<TGroupedItem>>();
        await foreach (var group in wrapped)
        {
            result.Add(group.Key, await group.ToListAsync());
        }
        return result;
    }
}
public static class GroupedItemsExtensions
{
    public static DictionaryGenerator<TGroupedItem> ToDictionaryGenerator<TGroupedItem>(this IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>> groups)
        where TGroupedItem : INamedItem 
            => new DictionaryGenerator<TGroupedItem>(groups);
}
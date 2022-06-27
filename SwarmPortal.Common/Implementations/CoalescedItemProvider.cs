using System.Runtime.CompilerServices;

namespace SwarmPortal.Common;

internal class CoalescedItemProvider<TItem> : ICoalescedItemProvider<TItem> 
    where TItem: INamedItem, IGroupableItem, IHasRoles
{
    private readonly IEnumerable<IItemProvider<TItem>> sources;

    public CoalescedItemProvider(IEnumerable<IItemProvider<TItem>> sources)
    {
        this.sources = sources;
    }
    public async IAsyncEnumerable<TItem> GetItems([EnumeratorCancellation] CancellationToken ct)
    {
        foreach (var source in sources)
        {
            await foreach (var item in source.GetItemsAsync(ct))
            {
                yield return item;
            }
        }
    }
}

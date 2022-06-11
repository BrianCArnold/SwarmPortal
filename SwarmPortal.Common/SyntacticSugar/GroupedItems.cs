namespace SwarmPortal.Common;

public abstract class GroupedItems<TGroupedItem> : IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>>
{
    private readonly IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>> wrapped;

    public GroupedItems(IAsyncEnumerable<IAsyncGrouping<string, TGroupedItem>> wrapped)
    {
        this.wrapped = wrapped;
    }

    public IAsyncEnumerator<IAsyncGrouping<string, TGroupedItem>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => wrapped.GetAsyncEnumerator(cancellationToken);
}
public class GroupedLinks : GroupedItems<IGroupedLinkItem>
{
    public GroupedLinks(IAsyncEnumerable<IAsyncGrouping<string, IGroupedLinkItem>> wrapped)
        : base(wrapped) {}
}
public class GroupedHosts : GroupedItems<IGroupedHostItem>
{
    public GroupedHosts(IAsyncEnumerable<IAsyncGrouping<string, IGroupedHostItem>> wrapped)
        : base(wrapped) {}
}
public static class GroupedExtensions
{
    public static GroupedHosts ToGroupedHosts(this IAsyncEnumerable<IAsyncGrouping<string, IGroupedHostItem>> toWrap)
     => new GroupedHosts(toWrap);
    public static GroupedLinks ToGroupedLinks(this IAsyncEnumerable<IAsyncGrouping<string, IGroupedLinkItem>> toWrap)
     => new GroupedLinks(toWrap);
}
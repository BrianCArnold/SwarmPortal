namespace SwarmPortal.Common;
public interface ILinkGroupProvider
{

    IAsyncEnumerable<IAsyncGrouping<string, IGroupedLinkItem>> GetLinkGroupsAsync();
}

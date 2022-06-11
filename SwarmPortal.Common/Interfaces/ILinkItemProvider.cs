namespace SwarmPortal.Common;
public interface ILinkItemProvider
{
    IAsyncEnumerable<ILinkItem> GetLinkItemsAsync();
}

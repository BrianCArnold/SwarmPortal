namespace SwarmPortal.Common;

public interface IHostGroupProvider
{
    IAsyncEnumerable<IAsyncGrouping<string, IGroupedHostItem>> GetHostGroupsAsync();
}
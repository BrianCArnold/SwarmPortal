namespace SwarmPortal.Common;
public interface IHostItemProvider
{
    IAsyncEnumerable<IHostItem> GetHostsAsync();
}
namespace SwarmPortal.Common;

public class CollatingHostGroupProvider : IHostGroupProvider
{
    private readonly IEnumerable<IHostItemProvider> providers;

    public CollatingHostGroupProvider(IEnumerable<IHostItemProvider> providers)
    {
        this.providers = providers;
    }

    public GroupedHosts GetHostGroupsAsync()
    {
        return providers.ToAsyncEnumerable()
            .SelectMany(p => p.GetHostsAsync())
            .OrderBy(i => i.Name)
            .GroupBy(i => i.Group)
            .ToGroupedHosts();
    }
}

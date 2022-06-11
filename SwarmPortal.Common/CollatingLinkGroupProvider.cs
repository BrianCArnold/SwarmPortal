namespace SwarmPortal.Common;

public class CollatingLinkGroupProvider : ILinkGroupProvider
{
    private readonly IEnumerable<ILinkItemProvider> providers;

    public CollatingLinkGroupProvider(IEnumerable<ILinkItemProvider> providers)
    {
        this.providers = providers;
    }

    public GroupedLinks GetLinkGroupsAsync()
    {
        return providers.ToAsyncEnumerable()
            .SelectMany(p => p.GetLinkItemsAsync())
            .OrderBy(i => i.Name)
            .GroupBy(i => i.Group)
            .ToGroupedLinks();
    }
}
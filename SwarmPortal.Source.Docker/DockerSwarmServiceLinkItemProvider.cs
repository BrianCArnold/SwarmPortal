namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceLinkItemProvider : DockerSwarmItemProvider<ILinkItem>
{

    private IEnumerable<string> SwarmPortalLabelPrefix => base.configuration.SwarmPortalLabelPrefix;

    public DockerSwarmServiceLinkItemProvider(ILogger<DockerSwarmServiceLinkItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
    {
    }

    public async override IAsyncEnumerable<ILinkItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Retrieving list of Docker Swarm Services from Docker Socket Client");
        var services = await client.Swarm.ListServicesAsync();
        logger.LogTrace("Iterating over Docker Swarm Services to construct Docker Service Links");
        var flattenedIteration = services.SelectMany(service => {
            logger.LogTrace("Processing Service...");
            return GetServiceItems(service);
        });
        foreach (var item in flattenedIteration)
        {
            yield return item;
        }
    }
    private IEnumerable<ILinkItem> GetServiceItems(SwarmService service)
    {
        logger.LogTrace("Setting up IEnumerable to skip labels we aren't looking for.");
        var swarmPortalLabels = service.Spec.Labels;

        // O(n) (where `n` is the number of period separated label words in all labels)
        HierarchichalDictionary<string> hierarchy = HierarchichalDictionary<string>.ConvertToHierarchy(service.Spec.Labels);
        
        HierarchichalDictionary<string>? portalLabelRoot = hierarchy.NavigateTo(SwarmPortalLabelPrefix);
        
        if (portalLabelRoot == null)
        {
            yield break;
        }

        // O(n) (where `n` is the number of period separated label words in all labels)
        foreach (var portalGroup in portalLabelRoot.Keys)
        {
            var groupName = portalGroup;
            var group = portalLabelRoot[groupName];
            foreach(var portalItem in group.Keys)
            {
                var itemName = portalItem;
                var item = group[itemName];
                if (item.ContainsChild(urlKey))
                {
                    var url = item[urlKey].Value!;
                    var role = item.ContainsChild(rolesKey) ? item[rolesKey].Value!.Split(',') : Enumerable.Empty<string>();
                    yield return new CommonLinkItem(itemName, groupName, url, role );
                }
            }
        }
    }
}

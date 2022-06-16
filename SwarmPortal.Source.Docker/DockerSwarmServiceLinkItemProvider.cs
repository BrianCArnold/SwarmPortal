namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceLinkItemProvider : DockerSwarmItemProvider<ILinkItem>
{
    private string SwarmPortalLabelPrefix
     => base.configuration.SwarmPortalLabelPrefix.EndsWith('.') ? base.configuration.SwarmPortalLabelPrefix : base.configuration.SwarmPortalLabelPrefix + ".";

    public DockerSwarmServiceLinkItemProvider(ILogger<DockerSwarmServiceLinkItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
    {
    }

    public async override IAsyncEnumerable<ILinkItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Retrieving list of Docker Swarm Services from Docker Socket Client");
        var services = await client.Swarm.ListServicesAsync();
        logger.LogTrace("Iterating over Docker Swarm Services to construct Docker Service Links");
        var flattenedIteration = services.ToAsyncEnumerable().SelectMany(service => {
            logger.LogTrace("Processing Service...");
            return GetServiceItems(service, ct);
        });
        await foreach (var item in flattenedIteration)
        {
            yield return item;
        }
    }
    private async IAsyncEnumerable<ILinkItem> GetServiceItems(SwarmService service, [EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Setting up IEnumerable to skip labels we aren't looking for.");
        var swarmPortalLabels = service.Spec.Labels.Where(l => l.Key.StartsWith(SwarmPortalLabelPrefix));
        //swarm.portal.GroupName.ServiceName.Url=https;(//)
        //swarm.portal.GroupName.ServiceName.Role=admin
        var swarmPortalLabelsForServiceInfo = swarmPortalLabels.Where(l => {
            //GroupName.ServiceName.Url
            var RemainderOfKey = l.Key.Substring(SwarmPortalLabelPrefix.Length);
            //[GroupName, ServiceName, Url]
            var splitUpRemainder = RemainderOfKey.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return splitUpRemainder.Length == 3;
        });
        var swarmPortalLabelsForServiceInfoGrouped = swarmPortalLabelsForServiceInfo
            .GroupBy(l => l.Key.Split('.', StringSplitOptions.RemoveEmptyEntries).Take(2));
        foreach (var labelGroup in swarmPortalLabelsForServiceInfoGrouped)
        {
                var group = labelGroup.Key.First();
                var name = labelGroup.Key.Skip(1).First();
                var roles = new List<string>();
                string url = string.Empty;
                foreach (var label in labelGroup)
                {
                    if (label.Key.Split('.').Skip(2).First().Equals("Role", StringComparison.InvariantCultureIgnoreCase))
                    {
                        roles.Add(label.Value);
                    }
                    else if (label.Key.Split('.').Skip(2).First().Equals("Url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        url = label.Value;
                    }
                }
                yield return new CommonLinkItem(name, group, url, roles);
        }
    }
}

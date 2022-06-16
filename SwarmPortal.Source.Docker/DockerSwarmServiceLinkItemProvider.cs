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
        var swarmPortalLabelGroups = swarmPortalLabels.GroupBy(l => l.Key.Substring(SwarmPortalLabelPrefix.Length).Split(".").Take(2).StringJoin("."));
        foreach (var labelGroup in swarmPortalLabelGroups)
        {
            if (labelGroup.Key.Split(".").Count() == 2)
            {
            var group = labelGroup.Key.Split(".").First();
            var name = labelGroup.Key.Split(".").Skip(1).First();
            var roles = new List<string>();
            string url = string.Empty;
            foreach (var label in labelGroup)
            {
                if (label.Key.Substring(SwarmPortalLabelPrefix.Length+labelGroup.Key.Length+1).StartsWith("Role"))
                {
                    roles.Add(label.Value);
                }
                else if (label.Key.Substring(SwarmPortalLabelPrefix.Length+labelGroup.Key.Length+1).StartsWith("Url"))
                {
                    url = label.Value;
                }
            }
            yield return new CommonLinkItem(name, group, url, roles);
            }
        }
    }
}

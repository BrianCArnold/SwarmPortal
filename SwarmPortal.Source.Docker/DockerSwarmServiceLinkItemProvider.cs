namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceLinkItemProvider : DockerSwarmItemProvider<ILinkItem>
{
    private const string SwarmPortalLabelPrefix = "swarm.portal.";

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
        var swarmPortalLabels = service.Spec.Labels.ToAsyncEnumerable().Where(l => l.Key.StartsWith(SwarmPortalLabelPrefix));
        await foreach (var label in swarmPortalLabels)
        {
            logger.LogTrace("Separating Label Key into period deliminated parts");
            var relevantKeyPortions = label.Key.Substring(SwarmPortalLabelPrefix.Length).Split(".");
            logger.LogTrace("Setting Link Group to first part.");
            var group = relevantKeyPortions.First();
            logger.LogTrace("Setting Link Name from remaining parts.");
            var name = relevantKeyPortions.Skip(1).StringJoin(" ");

            logger.LogTrace("Setting Link URL from label Value.");
            var url = label.Value;
            logger.LogInformation("Found Link Item.", new {
                Name = name,
                Group = group,
                URL = url
            });
            yield return new CommonLinkItem(name, group, url);
        }
    }
}

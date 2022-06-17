namespace SwarmPortal.Source.Docker;
public class DockerSwarmNodeStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private IEnumerable<string> SwarmPortalLabelPrefix => base.configuration.SwarmPortalLabelPrefix;

    public DockerSwarmNodeStatusItemProvider(ILogger<DockerSwarmNodeStatusItemProvider> logger, IDockerSourceConfiguration configuration)
     : base(logger, configuration)
    {
    }

    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Retrieving list of Docker Swarm Nodes from Docker Socket Client");
        var nodes = await client.Swarm.ListNodesAsync();
        const string group = "Docker Nodes";
        logger.LogTrace("Iterating over Docker Swarm Nodes to construct Docker Node Statuses");
        foreach (var node in nodes)
        {
            logger.LogTrace("Converting Labels to Hierarchy");
            LabelHierarchy hierarchy = LabelHierarchy.ConvertToHierarchy(node.Spec.Labels);
        
            logger.LogTrace("Navigating to relevant labels");
            LabelHierarchy? portalLabelRoot = hierarchy.NavigateTo(SwarmPortalLabelPrefix);

            var name = await GetNodeName(node);
            var status = await GetStatus(node);
            var roles = await GetRoles(portalLabelRoot);

            yield return new CommonStatusItem(name, group, status, roles);
        }
    }

    private async Task<string> GetNodeName(NodeListResponse node)
    {
        logger.LogTrace("Getting Node name from Description.Hostname");
        return node.Description.Hostname;
    }
    private async Task<IEnumerable<string>> GetRoles(LabelHierarchy? labelRoot)
    {
        logger.LogTrace("Getting Node Roles from Labels");
        if (labelRoot == null)
        {
            return Enumerable.Empty<string>();
        }

        return labelRoot.ContainsChild(rolesKey) ? labelRoot[rolesKey].Value.Split(',') : Enumerable.Empty<string>();
    }

    private async Task<Status> GetStatus(NodeListResponse node)
    {
        logger.LogTrace("Getting Node Status from Status.State and Spec.Availability.");
        return node.Status.State switch
        {
            "down" => Status.Offline,
            "ready" => node.Spec.Availability switch
            {
                "active" => Status.Online,
                "drain" => Status.Offline,
                "pause" => Status.Degraded,
                _ => Status.Unknown
            },
            _ => Status.Unknown
        };
    }
}

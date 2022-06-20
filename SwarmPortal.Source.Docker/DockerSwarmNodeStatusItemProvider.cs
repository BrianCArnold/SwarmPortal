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
            HierarchichalDictionary<string> hierarchy = HierarchichalDictionary<string>.ConvertToHierarchy(node.Spec.Labels);
        
            logger.LogTrace("Navigating to relevant labels");
            HierarchichalDictionary<string>? portalLabelRoot = hierarchy.NavigateTo(SwarmPortalLabelPrefix);

            var name = GetNodeName(node);
            var status = GetStatus(node);
            var roles = GetRoles(portalLabelRoot);

            yield return new CommonStatusItem(name, group, status, roles);
        }
    }

    private string GetNodeName(NodeListResponse node)
    {
        logger.LogTrace("Getting Node name from Description.Hostname");
        return node.Description.Hostname;
    }
    private IEnumerable<string> GetRoles(HierarchichalDictionary<string>? labelRoot)
    {
        logger.LogTrace("Getting Node Roles from Labels");
        if (labelRoot == null)
        {
            return Enumerable.Empty<string>();
        }
        else if (labelRoot.ContainsChild(rolesKey))
        {
            if (labelRoot[rolesKey].HasValue) 
            {
                var roles = labelRoot![rolesKey]!.Value!.Split(',').AsEnumerable();
                return roles;
            }
        }
        return Enumerable.Empty<string>();
    }

    private Status GetStatus(NodeListResponse node)
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

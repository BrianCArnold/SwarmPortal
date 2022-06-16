namespace SwarmPortal.Source.Docker;
public class DockerSwarmNodeStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private string SwarmPortalLabelPrefix
     => (base.configuration.SwarmPortalLabelPrefix.EndsWith('.') ? base.configuration.SwarmPortalLabelPrefix : base.configuration.SwarmPortalLabelPrefix + ".") + "role";

    public DockerSwarmNodeStatusItemProvider(ILogger<DockerSwarmNodeStatusItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
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
            logger.LogTrace("Processing node...");
            logger.LogTrace("Getting Node Name");
            string name = await GetNodeName(node);
            logger.LogTrace("Getting Node Name");
            Status status = await GetStatus(node);
            logger.LogTrace("Getting Node Roles");
            var roles = await GetNodeRoles(node);
            logger.LogInformation("Node Status: ", new { Name = name, Status = status });
            yield return new CommonStatusItem(name, group, status, roles);
        }
    }

    private async Task<IEnumerable<string>> GetNodeRoles(NodeListResponse node)
    {
        var roles = node.Spec.Labels.Where(l => 
        {
            return l.Key.StartsWith(SwarmPortalLabelPrefix);
        })
            .Select(l => l.Value);
        
        
        return roles;
    }
    private async Task<string> GetNodeName(NodeListResponse node)
    {
        logger.LogTrace("Getting Node name from Description.Hostname");
        return node.Description.Hostname;
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

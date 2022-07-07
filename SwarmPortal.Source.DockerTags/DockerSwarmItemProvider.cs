namespace SwarmPortal.Source.DockerTags;
public abstract class DockerSwarmItemProvider<TItem> : IItemProvider<TItem>
    where TItem : IGroupableItem
{
    protected const string urlKey = "url";
    protected const string rolesKey = "roles";
    protected readonly Uri dockerSocketUri;
    protected readonly DockerClientConfiguration clientConfig;
    protected readonly DockerClient client;
    protected readonly IDockerSourceConfiguration configuration;
    protected readonly ILogger<DockerSwarmItemProvider<TItem>> logger;

    //This works because the Type attribute for ILogger is *Contravariant*.
    // See "CoAndContravariance.txt" for more detail.
    public DockerSwarmItemProvider(ILogger<DockerSwarmItemProvider<TItem>> logger, IDockerSourceConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
        logger.LogTrace("Constructing Docker Socket URI");
        dockerSocketUri = new Uri(configuration.DockerSocketUri);
        logger.LogTrace("Constructing Docker Client Configuration");
        clientConfig = new DockerClientConfiguration(dockerSocketUri);
        logger.LogDebug("Creating Client from Docker Config");
        client = clientConfig.CreateClient();
        logger.LogInformation("Finished constructing Docker Client for Docker Socket");
    }


    public abstract IAsyncEnumerable<TItem> GetItemsAsync(CancellationToken ct);
}



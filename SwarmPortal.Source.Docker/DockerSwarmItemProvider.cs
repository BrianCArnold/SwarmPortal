namespace SwarmPortal.Source.Docker;
public abstract class DockerSwarmItemProvider<TItem> : IItemProvider<TItem>
    where TItem : IGroupableItem
{
    protected readonly Uri dockerSocketUri;
    protected readonly DockerClientConfiguration clientConfig;
    protected readonly DockerClient client;
    protected readonly IDockerSourceConfiguration configuration;
    protected readonly ILogger<DockerSwarmItemProvider<TItem>> logger;

    //This works because the Type attribute for ILogger is *Contravariant*.
    // When the constructor for a less derived type (a.k.a. parent class)
    //   is called by the more derived type, the generic ILogger for the more 
    //   derived type is accepted by typing by the less derived type because
    //   the generic type parameter is specified with the "out" keyword.
    //   > public interface ILogger<out TCategoryName> : ILogger
    // Any covariant type declaration must, by definition, only emit values,
    //   never accept them as parameters or as assignment.
    // See bottom of file for example.
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



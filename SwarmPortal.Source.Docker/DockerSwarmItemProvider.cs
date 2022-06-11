using System.Runtime.CompilerServices;
using Docker.DotNet;
using SwarmPortal.Common;

namespace SwarmPortal.Source.Docker;
public abstract class DockerSwarmItemProvider<TItem> : IItemProvider<TItem>
    where TItem : IGroupableItem
{
    protected readonly Uri dockerSocketUri;
    protected readonly DockerClientConfiguration clientConfig;
    protected readonly DockerClient client;

    public DockerSwarmItemProvider()
    {
        dockerSocketUri = new Uri("unix:///var/run/docker.sock");
        clientConfig = new DockerClientConfiguration(dockerSocketUri);
        client = clientConfig.CreateClient();
    }
    public abstract IAsyncEnumerable<TItem> GetItemsAsync(CancellationToken ct);
}

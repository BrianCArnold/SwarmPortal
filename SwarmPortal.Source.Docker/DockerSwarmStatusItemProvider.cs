using System.Runtime.CompilerServices;
using Docker.DotNet;
using SwarmPortal.Common;

namespace SwarmPortal.Source.Docker;
public abstract class DockerSwarmStatusItemProvider : IItemProvider<IStatusItem>
{
    protected readonly Uri dockerSocketUri;
    protected readonly DockerClientConfiguration clientConfig;
    protected readonly DockerClient client;

    public DockerSwarmStatusItemProvider()
    {
        dockerSocketUri = new Uri("unix:///var/run/docker.sock");
        clientConfig = new DockerClientConfiguration(dockerSocketUri);
        client = clientConfig.CreateClient();
    }
    public abstract IAsyncEnumerable<IStatusItem> GetItemsAsync(CancellationToken ct);
}

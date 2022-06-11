using System.Runtime.CompilerServices;
using Docker.DotNet;
using SwarmPortal.Common;

namespace SwarmPortal.Source.Docker;
public class DockerSwarmNodeStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        var nodes = await client.Swarm.ListNodesAsync();
        string group = "Nodes";
        foreach (var node in nodes)
        {
            string name = node.Description.Hostname;
            Status status = node.Status.State switch {
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
            yield return new CommonStatusItem(name, group, status);
        }
    }
}

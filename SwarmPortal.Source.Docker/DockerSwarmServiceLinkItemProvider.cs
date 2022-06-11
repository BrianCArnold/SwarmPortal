using System.Runtime.CompilerServices;
using SwarmPortal.Common;

namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceLinkItemProvider : DockerSwarmItemProvider<ILinkItem>
{
    private const string SwarmPortalLabelPrefix = "swarm.portal.";
    public async override IAsyncEnumerable<ILinkItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        
        var services = await client.Swarm.ListServicesAsync();
        foreach (var service in services)
        {
            var swarmPortalLabels = service.Spec.Labels.Where(l => l.Key.StartsWith(SwarmPortalLabelPrefix));
            foreach (var kvg in swarmPortalLabels.GroupBy(l => l.Key.Substring(SwarmPortalLabelPrefix.Length).Split(".").First()))
            {
                var group = kvg.Key;
                foreach (var label in kvg)
                {
                    var name = label.Key.Substring(SwarmPortalLabelPrefix.Length+group.Length+1);
                    var url = label.Value;
                    yield return new CommonLinkItem(name, group, url);
                }
            }
        }
    }
}

using System.Runtime.CompilerServices;
using Docker.DotNet;
using Newtonsoft.Json;
using SwarmPortal.Common;

namespace SwarmPortal.Source.Docker;
public class DockerSwarmServiceStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private const string StackNameLabel = "com.docker.stack.namespace";
    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        var services = await client.Swarm.ListServicesAsync();
        
        foreach (var service in services)
        {
            string stack;
            string name;
            var serviceName = service.Spec.Name;
            if (service.Spec.Labels.ContainsKey(StackNameLabel))
            {
                stack = service.Spec.Labels[StackNameLabel];
                if (serviceName.StartsWith(stack+"_"))
                {
                    name = serviceName.Substring(stack.Length+1);
                }
                else 
                {
                    name = serviceName;
                }
            }
            else
            {
                stack = "None";
                name = serviceName;
            }
            var inspectData = await client.Swarm.InspectServiceAsync(service.ID);
            
            Console.WriteLine(JsonConvert.SerializeObject(inspectData));
            // ulong running = inspectData.ServiceStatus.RunningTasks;
            // ulong desired = inspectData.ServiceStatus.DesiredTasks;

            // Status status = desired switch {
            //     0ul => Status.Unknown,
            //     _ => running switch {
            //         0 => Status.Offline,
            //         _ when running < desired => Status.Degraded,
            //         _ => Status.Online
            //     }
            // };
            Status status = Status.Unknown;

            yield return new CommonStatusItem(name, stack, status);
        }
    }
}

namespace SwarmPortal.Source.Docker;
public class DockerSwarmServiceStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private const string StackNameLabel = "com.docker.stack.namespace";

    public DockerSwarmServiceStatusItemProvider(ILogger<DockerSwarmServiceStatusItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
    {
    }

    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Retrieving list of Docker Swarm Services from Docker Socket Client");
        var services = await client.Swarm.ListServicesAsync();
        logger.LogTrace("Iterating over Docker Swarm Services to construct Docker Service Statuses");
        foreach (var service in services)
        {
            logger.LogTrace("Getting Stack and Service Name");
            var (stack, serviceName) = await GetStackAndServiceName(service);
            
            Status status = await GetStatus(service);
            yield return new CommonStatusItem(serviceName, stack, status, Enumerable.Empty<string>());
        }
    }

    private async Task<Status> GetStatus(SwarmService service)
    {
        Status status;
        
        logger.LogTrace("Checking to make sure SwarmService has the required information.");
        SwarmService filledOutService;
        if (service.ServiceStatus == null)
        {
            logger.LogInformation("Retrieving SwarmService information from Docker");
            filledOutService = await client.Swarm.InspectServiceAsync(service.ID);
        }
        else 
        {
            logger.LogTrace("Using existing data, appears to be correct");
            filledOutService = service;
        }
        
        if (filledOutService.ServiceStatus != null)
        {
        // // Console.WriteLine(JsonConvert.SerializeObject(inspectData));
            logger.LogTrace("Getting number of running Service Tasks");
            ulong running = filledOutService.ServiceStatus.RunningTasks;
            logger.LogTrace("Getting number of desired Service Tasks");
            ulong desired = filledOutService.ServiceStatus.DesiredTasks;

            
            logger.LogTrace("Determining Service Status");
            status = desired switch {
                0x0ul => Status.Offline,
                _ => running switch {
                    0x0ul => Status.Offline,
                    _ when running < desired => Status.Degraded,
                    _ when running == desired => Status.Online,
                    _ when running > desired => Status.Unknown,
                    //This shouldn't even happen, but the compiler
                    // seems to think that there's a case that's not handled?
                    // Just in case I've missed something, I'll return Unknown.
                    _ => Status.Unknown
                }
            };
        }
        else
        {
            /* 
                It looks like for whatever reason, this is getting back null for the service status.
                My guess is that it's out of date from the current version of Docker running locally in Dev.
                Need to spin up some VMs with older versions of Docker to confirm.
                For now, just returning a status of "Unknown".
            */
            logger.LogWarning("All Service Statuses are set to 'Unknown' due to bug in DotNet.Docker.");
            status = Status.Unknown;
        }
        logger.LogInformation("Retrieved Status", new { Status = status });
        return status;
    }

    private Task<(string stack, string service)>  GetStackAndServiceName(SwarmService swarmService)
    {
        string stack;
        string service;
        
        logger.LogTrace("Checking if Service has a Stack Namespace Label.");
        if (swarmService.Spec.Labels.ContainsKey(StackNameLabel))
        {
            
            logger.LogInformation("Setting name of stack from Stack Namespace Label");
            stack = swarmService.Spec.Labels[StackNameLabel];
            
            logger.LogTrace("Checking if Service Name matches Stack Name");
            if (swarmService.Spec.Name.StartsWith(stack + "_"))
            {//I think this should always be the case?
            
                logger.LogInformation("Setting name of service");
                service = swarmService.Spec.Name.Substring(stack.Length + 1);
            }
            else
            {
                logger.LogInformation("Setting name of service");
                logger.LogWarning("Setting name of service: STACK NAME MISSING.");
                service = swarmService.Spec.Name;
            }
        }
        else
        {
            logger.LogInformation("Setting name of stack to 'None'");
            stack = "None";
            logger.LogInformation("Setting name of service");
            service = swarmService.Spec.Name;
        }
        logger.LogInformation("Found Service Status Item.", new {
            StackName = stack,
            ServiceName = service
        });
        return Task.FromResult((stack, service));
    }
}

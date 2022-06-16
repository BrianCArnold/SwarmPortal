namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private const string StackNameLabel = "com.docker.stack.namespace";

    public DockerSwarmServiceStatusItemProvider(ILogger<DockerSwarmServiceStatusItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
    {
        
    }

    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        Dictionary<string, Dictionary<TaskState, int>> taskDictionary = await GetTaskDictionary();
        taskDictionary.ToString();
        logger.LogTrace("Retrieving list of Docker Swarm Services from Docker Socket Client");
        var servicesTask = client.Swarm.ListServicesAsync(null, ct);
        var services = await servicesTask;
        logger.LogTrace("Iterating over Docker Swarm Services to construct Docker Service Statuses");
        foreach (var service in services)
        {
            logger.LogTrace("Getting Stack and Service Name");
            var (stack, serviceName) = await GetStackAndServiceName(service);

            Dictionary<TaskState, int> taskStates = taskDictionary[service.ID] ?? new();
            Status status = await GetStatus(service, taskStates);
            yield return new CommonStatusItem(serviceName, stack, status, Enumerable.Empty<string>());
        }
    }

    private async Task<Dictionary<string, Dictionary<TaskState, int>>> GetTaskDictionary()
    {
        var serviceTasks = await client.Tasks.ListAsync();
        var taskDictionary = serviceTasks.GroupBy(t => t.ServiceID)
            .ToDictionary(g => g.Key, g => g.GroupBy(t => t.Status.State).ToDictionary(t => t.Key, t => t.Count()));
        return taskDictionary;
    }

    private async Task<Status> GetStatus(SwarmService service, Dictionary<TaskState, int> states)
    {
        Status status;
        if (!states.Any())
        {
            status = Status.Unknown;
        }
        else 
        {
        // // Console.WriteLine(JsonConvert.SerializeObject(inspectData));
            logger.LogTrace("Getting number of running Service Tasks");
            ulong running = states.ContainsKey(TaskState.Running) ? (ulong)states[TaskState.Running] : 0UL;
            logger.LogTrace("Getting number of desired Service Tasks");
            ulong desired = service.Spec.Mode.Replicated.Replicas ?? 0;

            
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

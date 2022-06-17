namespace SwarmPortal.Source.Docker;

public class DockerSwarmServiceStatusItemProvider : DockerSwarmItemProvider<IStatusItem>
{
    private const string StackNameLabel = "com.docker.stack.namespace";

    private IEnumerable<string> SwarmPortalLabelPrefix => base.configuration.SwarmPortalLabelPrefix;

    public DockerSwarmServiceStatusItemProvider(ILogger<DockerSwarmServiceStatusItemProvider> logger, IDockerSourceConfiguration configuration) : base(logger, configuration)
    {
        
    }

    public override async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        logger.LogTrace("Retrieving list of Docker Swarm Nodes from Docker Socket Client");
        var nodesTask = client.Swarm.ListNodesAsync(ct);
        logger.LogTrace("Retrieving list of Docker Swarm Services from Docker Socket Client");
        var servicesTask = client.Swarm.ListServicesAsync(null, ct);
        logger.LogTrace("Retrieving list of Docker Swarm Tasks from Docker Socket Client");
        var taskDictionaryTask = GetTaskDictionary();
        var nodes = await nodesTask;
        var services = await servicesTask;
        var taskDictionary = await taskDictionaryTask;


        var activeNodes = nodes.Select(n => n.Status.State == "ready" && n.Spec.Availability == "active").Count();
        logger.LogTrace("Iterating over Docker Swarm Services to construct Docker Service Statuses");
        foreach (var service in services)
        {
            logger.LogTrace("Getting Stack and Service Name");
            Dictionary<TaskState, int> taskStates = taskDictionary[service.ID] ?? new();

            // O(n) (where `n` is the number of period separated label words in all labels)
            LabelHierarchy hierarchy = LabelHierarchy.ConvertToHierarchy(service.Spec.Labels);
            
            LabelHierarchy? portalLabelRoot = hierarchy.NavigateTo(SwarmPortalLabelPrefix);

            var stackAndServiceNameTask = GetStackAndServiceName(service);
            var statusTask = GetStatus(activeNodes, service, taskStates);
            var rolesTask = GetRoles(portalLabelRoot);

            Status status = await statusTask;
            var (stack, serviceName) = await stackAndServiceNameTask;
            var roles = await rolesTask;
            yield return new CommonStatusItem(serviceName, stack, status, roles);
        }
    }

    private async Task<IEnumerable<string>> GetRoles(LabelHierarchy? labelRoot)
    {
        logger.LogTrace("Getting Node Roles from Labels");
        if (labelRoot == null)
        {
            return Enumerable.Empty<string>();
        }
        return labelRoot.ContainsChild(rolesKey) ? labelRoot[rolesKey].Value.Split(',') : Enumerable.Empty<string>();
    }
    
    private async Task<Dictionary<string, Dictionary<TaskState, int>>> GetTaskDictionary()
    {
        var serviceTasks = await client.Tasks.ListAsync();
        var taskDictionary = serviceTasks.GroupBy(t => t.ServiceID)
            .ToDictionary(g => g.Key, g => g.GroupBy(t => t.Status.State).ToDictionary(t => t.Key, t => t.Count()));
        return taskDictionary;
    }

    private async Task<Status> GetStatus(int activeNodes, SwarmService service, Dictionary<TaskState, int> states)
    {
        try 
        {
            Status status;
            if (!states.Any())
            {
                logger.LogTrace("No Tasks found for Service");
                status = Status.Unknown;
            }
            else 
            {
                logger.LogTrace("Getting number of running Service Tasks");
                ulong running = states.ContainsKey(TaskState.Running) ? (ulong)states[TaskState.Running] : 0UL;
                logger.LogTrace("Getting number of desired Service Tasks");
                ulong desired;
                if (service.Spec.Mode.Replicated != null)
                {
                    logger.LogTrace("Service is Replicated");
                    desired = service.Spec.Mode.Replicated.Replicas ?? 0;
                }
                else
                {
                    logger.LogTrace("Service is Global");
                    desired = (ulong)activeNodes;
                }
                
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
        catch(Exception e)
        {
            logger.LogError(e, "Error getting status");
            return Status.Unknown;
        }
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
        return Task.FromResult(("Stack: " + stack, service));
    }
}

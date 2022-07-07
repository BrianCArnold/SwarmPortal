using Microsoft.Extensions.Configuration;

namespace SwarmPortal.Source.DockerTags;

public class DockerSourceConfiguration : IDockerSourceConfiguration
{
    public static IDockerSourceConfiguration Create(IConfiguration config)
    {
        var dockerSourceConfig = new DockerSourceConfiguration();
        config.Bind(dockerSourceConfig);
        return dockerSourceConfig;
    }
    private  DockerSourceConfiguration()
    {
    }
    public string DockerSocketUri { get; set; } = "unix:///var/run/docker.sock";
    public IEnumerable<string> SwarmPortalLabelPrefix { get; set; } = new List<string>{ "swarm","portal" };
    public string NodeRole { get; set; } = "swarm.portal.node.role";
}
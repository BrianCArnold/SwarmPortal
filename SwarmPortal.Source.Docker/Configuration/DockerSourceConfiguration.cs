using Microsoft.Extensions.Configuration;

namespace SwarmPortal.Source;

internal class DockerSourceConfiguration : IDockerSourceConfiguration
{
    internal static IDockerSourceConfiguration Create(IConfiguration config)
    {
        var dockerSourceConfig = new DockerSourceConfiguration();
        config.Bind(dockerSourceConfig);
        return dockerSourceConfig;
    }
    private  DockerSourceConfiguration()
    {
    }
    public string DockerSocketUri { get; set; } = "unix:///var/run/docker.sock";
    public string SwarmPortalLabelPrefix { get; set; } = "swarm.portal";
}
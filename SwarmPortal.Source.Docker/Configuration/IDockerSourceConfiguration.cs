namespace SwarmPortal.Source;

public interface IDockerSourceConfiguration
{
    string SwarmPortalLabelPrefix { get; }
    string DockerSocketUri { get; }
}

namespace SwarmPortal.Source;

public interface IDockerSourceConfiguration
{
    IEnumerable<string> SwarmPortalLabelPrefix { get; }
    string DockerSocketUri { get; }
}

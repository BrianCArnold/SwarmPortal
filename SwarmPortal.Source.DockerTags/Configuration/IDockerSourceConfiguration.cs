namespace SwarmPortal.Source.DockerTags;

public interface IDockerSourceConfiguration
{
    IEnumerable<string> SwarmPortalLabelPrefix { get; }
    string DockerSocketUri { get; }
}

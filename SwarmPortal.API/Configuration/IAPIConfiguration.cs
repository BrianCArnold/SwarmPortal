namespace SwarmPortal.Common;
public interface IAPIConfiguration
{
    bool EnableStaticFileLinks { get; }
    bool EnableDockerNodeStatus { get; }
    bool EnableDockerServiceStatus { get; }
    bool EnableDockerServiceLinks { get; }
}

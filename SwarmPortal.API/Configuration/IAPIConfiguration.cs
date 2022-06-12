namespace SwarmPortal.Common;
public interface IAPIConfiguration
{
    bool EnableStaticLinks { get; }
    bool EnableStaticStatus { get; }
    bool EnableDockerNodeStatus { get; }
    bool EnableDockerServiceStatus { get; }
    bool EnableDockerServiceLinks { get; }
}

namespace SwarmPortal.Common;

public class APIConfiguration : IAPIConfiguration
{
    public static IAPIConfiguration Create(IConfiguration config)
    {
        var apiConfiguration = new APIConfiguration();
        config.Bind(apiConfiguration);
        return apiConfiguration;
    }
    private APIConfiguration()
    {
    }
    //Does this configuration need to be handled by the libraries that provide these sources? Not certain, will revisit.
    public bool EnableStaticLinks { get; set; } = false;
    public bool EnableStaticStatus { get; set; } = false;
    public bool EnableDockerNodeStatus { get; set; } = true;
    public bool EnableDockerServiceStatus { get; set; } = true;
    public bool EnableDockerServiceLinks { get; set; } = true;
}

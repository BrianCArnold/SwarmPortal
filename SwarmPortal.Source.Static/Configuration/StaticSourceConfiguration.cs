using Microsoft.Extensions.Configuration;

namespace SwarmPortal.Source;

public class StaticSourceConfiguration : IStaticSourceConfiguration
{
    public static IStaticSourceConfiguration Create(IConfiguration config)
    {
        var dockerSourceConfig = new StaticSourceConfiguration();
        config.Bind(dockerSourceConfig);
        return dockerSourceConfig;
    }
    private  StaticSourceConfiguration()
    {
    }
    public string StaticLinksFileName { get; set; } = "persist/links.json";
}
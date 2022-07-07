namespace SwarmPortal.ForwardAuth;

public class ForwardAuthConfig
{
    public string JwtSecret { get; set; } = null!;
    public string JwtRefreshSecret { get; set; } = null!;
    public string FQDN { get; set; } = null!;
    public string Scope { get; set; } = null!;
}

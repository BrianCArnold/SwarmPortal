namespace SwarmPortal.ForwardAuth;

public class ForwardAuthConfig
{
    public string JwtSecret { get; set; }
    public string JwtRefreshSecret { get; set; }
    public string FQDN { get; set; }
    public string Scope { get; set; }
}
public class OAuthClient
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string[] ClientScope { get; set; }
    public string IssuerUrl { get; set; }
}
public class OAuthClientRegistry : Dictionary<string, OAuthClient>
{
}
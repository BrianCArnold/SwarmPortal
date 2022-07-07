namespace SwarmPortal.ForwardAuth;

public class OAuthClient
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string[] ClientScope { get; set; } = null!;
    public string IssuerUrl { get; set; } = null!;
}

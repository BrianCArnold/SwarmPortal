namespace SwarmPortal.Common;

public interface IAuthConfig
{
    string Authority { get; set; }
    string Audience { get; set; }
    string Issuer { get; set; }
    string ClientId { get; set; }
    string RedirectUri { get; set; }
    string Scope { get; set; }
    bool RequireHttps { get; set; }
}
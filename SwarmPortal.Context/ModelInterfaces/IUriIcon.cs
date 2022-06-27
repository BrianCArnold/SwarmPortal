namespace SwarmPortal.Context;

public interface IUriIcon
{
    public ulong Id { get; set; }
    public Uri Uri { get; set; }
    public string Icon { get; set; }
}

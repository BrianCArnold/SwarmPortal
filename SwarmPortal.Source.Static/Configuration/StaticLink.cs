namespace SwarmPortal.Static;

public class StaticLink
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}
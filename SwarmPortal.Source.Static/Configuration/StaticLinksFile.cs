namespace SwarmPortal.Static;
public class StaticLinksFile
{
    public Dictionary<string, IEnumerable<StaticLink>> Groups { get; set; } = new();
}

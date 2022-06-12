
namespace SwarmPortal.Source.SQLite;
public class Link
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Group Group { get; set; }
}

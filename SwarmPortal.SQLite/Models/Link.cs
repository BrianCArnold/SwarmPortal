
namespace SwarmPortal.SQLite;
public class Link
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Group Group { get; set; }
    public SwarmPortalUser User { get; set; }
}

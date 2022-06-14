namespace SwarmPortal.Common;

public interface INamedItem
{
    string Name { get; }
    IEnumerable<string> Roles { get; }
}

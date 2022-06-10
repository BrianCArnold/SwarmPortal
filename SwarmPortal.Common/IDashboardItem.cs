namespace SwarmPortal.Common;
public interface IDashboardItem
{
    string Group { get; }
    string Name { get; }
    string Url { get; }
}
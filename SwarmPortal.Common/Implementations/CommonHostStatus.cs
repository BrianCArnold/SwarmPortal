namespace SwarmPortal.Common;
public record CommonHostItem(string Name, string Group, Status Status) : IGroupableHostItem
{
    public IHostItem StripGroup() => new CommonGroupedHostItem(Name, Status);
}
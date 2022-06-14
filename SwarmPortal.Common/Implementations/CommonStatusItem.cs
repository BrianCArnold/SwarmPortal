namespace SwarmPortal.Common;
public record CommonStatusItem(string Name, string Group, Status Status, IEnumerable<string> Roles) : IStatusItem;
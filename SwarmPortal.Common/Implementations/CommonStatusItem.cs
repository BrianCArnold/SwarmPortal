namespace SwarmPortal.Common;
public record CommonStatusItem(string Name, string Group, Status Status) : IStatusItem;
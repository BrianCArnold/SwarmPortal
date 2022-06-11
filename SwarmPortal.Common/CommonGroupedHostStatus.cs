namespace SwarmPortal.Common;

public record CommonGroupedHostStatus(string Name, string Group, Status Status) : IHostItem;

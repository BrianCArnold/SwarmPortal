namespace SwarmPortal.Common;
public record CommonLinkItem(string Name, string Group, string Url, IEnumerable<string> Roles) : ILinkItem;
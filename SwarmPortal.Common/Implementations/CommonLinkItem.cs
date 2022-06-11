namespace SwarmPortal.Common;
public record CommonLinkItem(string Name, string Group, string Url) : IGroupableLinkItem
{
    public ILinkItem StripGroup() => new CommonGroupedLinkItem(Name, Url);
}
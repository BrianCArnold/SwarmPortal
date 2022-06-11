namespace SwarmPortal.Common;

public interface ILinkItem : INamedItem, IGroupableItem
{
    string Url { get; }
}

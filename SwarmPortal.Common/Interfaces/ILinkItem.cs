namespace SwarmPortal.Common;

public interface ILinkItem : INamedItem
{
    string Url { get; }
    string Name { get; }
}

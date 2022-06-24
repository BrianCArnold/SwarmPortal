namespace SwarmPortal.Common;

public interface ILinkItem : INamedItem, IGroupableItem, IHasRoles
{
    string Url { get; }
}

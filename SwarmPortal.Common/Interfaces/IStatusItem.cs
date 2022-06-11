namespace SwarmPortal.Common;

public interface IStatusItem : INamedItem, IGroupableItem
{
    Status Status { get; }
}
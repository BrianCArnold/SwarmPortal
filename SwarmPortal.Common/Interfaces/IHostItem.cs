namespace SwarmPortal.Common;

public interface IHostItem : INamedItem, IGroupableItem
{
    Status Status { get; }
}
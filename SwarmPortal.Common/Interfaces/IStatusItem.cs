namespace SwarmPortal.Common;

public interface IStatusItem : INamedItem, IGroupableItem, IHasRoles
{
    Status Status { get; }
}
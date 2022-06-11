namespace SwarmPortal.Common;

public interface IHostItem : INamedItem
{
    Status Status { get; }
}
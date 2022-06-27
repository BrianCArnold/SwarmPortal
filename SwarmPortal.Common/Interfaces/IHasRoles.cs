namespace SwarmPortal.Common;

public interface IHasRoles
{
    IEnumerable<string> Roles { get; }
}

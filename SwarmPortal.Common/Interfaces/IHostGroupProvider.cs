namespace SwarmPortal.Common;

public interface IHostGroupProvider
{
    GroupedHosts GetHostGroupsAsync();
}
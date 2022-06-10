namespace SwarmPortal.Common;
public interface IHostStatusProvider
{
    Task<IEnumerable<IHostStatus>> GetHosts();
}
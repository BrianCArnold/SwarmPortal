using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticHostStatusProvider : IHostStatusProvider
{
    public async Task<IEnumerable<IHostStatus>> GetHosts() 
        => new []{
            new CommonHostStatus("Online Host One", Status.Online),
            new CommonHostStatus("Online Host Two", Status.Online),
            new CommonHostStatus("Offline Host Three", Status.Offline),
            new CommonHostStatus("Online Host Four", Status.Online),
        };
}

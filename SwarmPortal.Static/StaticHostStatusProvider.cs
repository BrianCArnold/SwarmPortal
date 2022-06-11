using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticHostStatusProvider : IHostItemProvider
{
    //This is basically just a mock up of something that takes a while to get individual items.
    public async IAsyncEnumerable<IHostItem> GetHostsAsync()
    {    
        await Task.Delay(10);
        yield return new CommonHostStatus("Online Host One", "Group B", Status.Online);
        await Task.Delay(10);
        yield return new CommonHostStatus("Online Host Two", "Group A", Status.Online);
        await Task.Delay(10);
        yield return new CommonHostStatus("Offline Host Three", "Group A", Status.Offline);
        await Task.Delay(10);
        yield return new CommonHostStatus("Online Host Four", "Group B", Status.Online);
        await Task.Delay(10);
    }
}

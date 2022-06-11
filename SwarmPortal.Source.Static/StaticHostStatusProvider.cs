using System.Runtime.CompilerServices;
using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticHostItemProvider : IItemProvider<IHostItem>
{
    //This is basically just a mock up of something that takes a while to get individual items.

    public async IAsyncEnumerable<IHostItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Delay(10);
        yield return new CommonHostItem("Online Host One", "Group B", Status.Online);
        await Task.Delay(10);
        yield return new CommonHostItem("Online Host Two", "Group A", Status.Online);
        await Task.Delay(10);
        yield return new CommonHostItem("Offline Host Three", "Group A", Status.Offline);
        await Task.Delay(10);
        yield return new CommonHostItem("Online Host Four", "Group B", Status.Online);
        await Task.Delay(10);
    }
}

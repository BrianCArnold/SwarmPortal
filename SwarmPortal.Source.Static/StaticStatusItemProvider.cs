using System.Runtime.CompilerServices;
using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticStatusItemProvider : IItemProvider<IStatusItem>
{
    //This is basically just a mock up of something that takes a while to get individual items.

    public async IAsyncEnumerable<IStatusItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Delay(10);
        yield return new CommonStatusItem("Online Host One", "Group B", Status.Online);
        await Task.Delay(10);
        yield return new CommonStatusItem("Online Host Two", "Group A", Status.Online);
        await Task.Delay(10);
        yield return new CommonStatusItem("Offline Host Three", "Group A", Status.Offline);
        await Task.Delay(10);
        yield return new CommonStatusItem("Online Host Four", "Group B", Status.Online);
        await Task.Delay(10);
    }
}

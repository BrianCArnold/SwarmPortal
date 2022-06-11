using System.Runtime.CompilerServices;
using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticLinkItemProvider : IItemProvider<IGroupableLinkItem>
{

    //This is basically just a mock up of something that takes a while to get individual items.
    public async IAsyncEnumerable<IGroupableLinkItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Delay(10);
        yield return new CommonLinkItem("Twitter", "Social", "https://twitter.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Ask!", "Search", "https://ask.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Bing", "Search", "https://bing.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Yahoo!", "Search", "https://yahoo.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Facebook", "Social", "https://facebook.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Google", "Search", "https://google.com");
        await Task.Delay(10);
    }
}

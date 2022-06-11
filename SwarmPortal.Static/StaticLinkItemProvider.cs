using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticLinkItemProvider : ILinkItemProvider
{

    //This is basically just a mock up of something that takes a while to get individual items.
    public async IAsyncEnumerable<ILinkItem> GetLinkItemsAsync()
    {
        await Task.Delay(10);
        yield return new CommonLinkItem("Google", "Search", "https://google.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Ask Jeeves", "Search", "https://ask.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Bing", "Search", "https://bing.com");
        await Task.Delay(10);
        yield return new CommonLinkItem("Yahoo!", "Search", "https://yahoo.com");
        await Task.Delay(10);
    }
}

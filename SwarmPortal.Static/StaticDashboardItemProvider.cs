using SwarmPortal.Common;
namespace SwarmPortal.Static;

public class StaticDashboardItemProvider : IDashboardItemProvider
{
    public async Task<IEnumerable<IDashboardItem>> GetDashboardItems() 
        => new []{
            new CommonDashboardItem("Google", "Search", "https://google.com"),
            new CommonDashboardItem("Ask Jeeves", "Search", "https://ask.com"),
            new CommonDashboardItem("Bing", "Search", "https://bing.com"),
            new CommonDashboardItem("Yahoo!", "Search", "https://yahoo.com"),
        };
}

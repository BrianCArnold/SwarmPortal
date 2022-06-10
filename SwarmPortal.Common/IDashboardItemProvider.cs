namespace SwarmPortal.Common;
public interface IDashboardItemProvider
{
    Task<IEnumerable<IDashboardItem>> GetDashboardItems();
}

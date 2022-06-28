namespace SwarmPortal.Context;

public interface IUriIconAccessor
{
    Task<IEnumerable<IUriIcon>> GetUriIcons(CancellationToken ct = default);
    Task<IUriIcon> GetUriIcon(Uri uri, CancellationToken ct = default);
    Task<IUriIcon> UpsertUriIcon(Uri uri, Uri icon, CancellationToken ct = default);
    Task<bool> ContainsUriIcon(Uri uri, CancellationToken ct = default);
    Task DeleteUriIcon(Uri uri, CancellationToken ct = default);
}

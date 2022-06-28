namespace SwarmPortal.Common;

public interface IIconProvider
{
    ValueTask<Stream> GetIcon(Uri uri, CancellationToken ct = default);
}
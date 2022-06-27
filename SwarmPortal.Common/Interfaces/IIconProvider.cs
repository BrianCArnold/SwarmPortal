namespace SwarmPortal.Common;

public interface IIconProvider
{
    ValueTask<FileStream?> GetIcon(Uri uri, CancellationToken ct = default);
}
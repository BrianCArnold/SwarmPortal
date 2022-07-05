namespace SwarmPortal.Common;

public interface IIconProvider
{
    ValueTask<IconSuccess> GetIcon(Uri uri, CancellationToken ct = default);
}
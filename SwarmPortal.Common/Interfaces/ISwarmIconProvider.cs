public interface IIconProvider
{
    ValueTask<string> GetIcon(Uri uri);
}
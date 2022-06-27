using System.Diagnostics.CodeAnalysis;

public class UriIcon
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public Uri Uri { get; set; } = null!;
    [NotNull]
    public string Icon { get; set; } = null!;
}
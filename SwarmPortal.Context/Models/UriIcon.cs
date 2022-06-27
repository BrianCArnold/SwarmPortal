namespace SwarmPortal.Context;

using System.Diagnostics.CodeAnalysis;

public class UriIcon : IUriIcon
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public Uri Uri { get; set; } = null!;
    [NotNull]
    public Uri Icon { get; set; } = null!;
    public DateTime RetrievedDate { get; set; }
}
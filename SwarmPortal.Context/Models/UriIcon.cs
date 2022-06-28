namespace SwarmPortal.Context;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Uri), IsUnique = true)]
public class UriIcon : IUriIcon
{
    [NotNull]
    public ulong Id { get; set; }
    [NotNull]
    public Uri Uri { get; set; } = null!;
    [NotNull]
    public Uri Icon { get; set; } = null!;
    [NotNull]
    public DateTime RetrievedDate { get; set; } = DateTime.UtcNow;
}
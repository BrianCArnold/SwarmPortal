namespace System.Linq;
public static class SwarmPortalExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> sets)
     => sets.SelectMany(x => x);
}
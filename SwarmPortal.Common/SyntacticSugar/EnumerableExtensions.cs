namespace System.Linq;
public static class SwarmPortalExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> sets)
     => sets.SelectMany(x => x);
    public static string StringJoin(this IEnumerable<string> set, string joiner)
     => string.Join(joiner, set);
}
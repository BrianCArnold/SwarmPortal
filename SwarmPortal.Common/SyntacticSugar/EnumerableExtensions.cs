using System.Security.Claims;

namespace System.Linq;
public static class SwarmPortalExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> sets)
     => sets.SelectMany(x => x);
    public static string StringJoin(this IEnumerable<string> set, string joiner)
     => string.Join(joiner, set);

    //HashSet specifically, because the lookup is faster than a dictionary for seeing if someone has a role.
    public static HashSet<string> GetRoles(this IEnumerable<Claim> claims)
      => claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToHashSet();
}
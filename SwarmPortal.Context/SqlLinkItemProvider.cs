using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwarmPortal.Common;

namespace SwarmPortal.Context;

public class SqlLinkItemProvider : IItemProvider<ILinkItem>
{
    private readonly SourceContext context;
    private readonly ILogger<SqlLinkItemProvider> logger;

    public SqlLinkItemProvider(SourceContext context, ILogger<SqlLinkItemProvider> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public IAsyncEnumerable<ILinkItem> GetItemsAsync(CancellationToken ct)
     => context.Links
        .Include(l => l.Group)
        .Include(l => l.Roles)
        .Where(l => l.Enabled && l.Group.Enabled)
        .AsAsyncEnumerable()
        .Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url, l.Roles.Where(r => r.Enabled).Select(r => r.Name)));
}

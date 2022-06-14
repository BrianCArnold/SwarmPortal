using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwarmPortal.Common;
using SwarmPortal.Source;

namespace SwarmPortal.SQLite;

public class SQLiteFileLinkItemProvider : IItemProvider<ILinkItem>
{
    private readonly SourceContext context;
    private readonly ILogger<SQLiteFileLinkItemProvider> logger;
    private readonly ISQLiteSourceConfiguration configuration;

    public SQLiteFileLinkItemProvider(SourceContext context, ILogger<SQLiteFileLinkItemProvider> logger, ISQLiteSourceConfiguration configuration)
    {
        this.context = context;
        this.logger = logger;
        this.configuration = configuration;
    }

    //This is basically just a mock up of something that takes a while to get individual items.
    public IAsyncEnumerable<ILinkItem> GetItemsAsync(CancellationToken ct)
     => context.Links.Include(l => l.Group)
                    .Select(l => new CommonLinkItem(l.Name, l.Group.Name, l.Url))
                    .AsAsyncEnumerable();
}

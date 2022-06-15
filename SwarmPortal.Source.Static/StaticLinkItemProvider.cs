using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwarmPortal.Common;
using SwarmPortal.Source;

namespace SwarmPortal.Static;

public class StaticFileLinkItemProvider : IItemProvider<ILinkItem>
{
    private readonly ILogger<StaticFileLinkItemProvider> logger;
    private readonly IStaticSourceConfiguration configuration;

    public StaticFileLinkItemProvider(ILogger<StaticFileLinkItemProvider> logger, IStaticSourceConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    //This is basically just a mock up of something that takes a while to get individual items.
    public async IAsyncEnumerable<ILinkItem> GetItemsAsync([EnumeratorCancellation] CancellationToken ct)
    {
        // .NET will only create a directory if it doesn't exist.
        // This is idempodent in regards to the directory.
        Directory.CreateDirectory(Path.GetDirectoryName(configuration.StaticLinksFileName));
        if (!File.Exists(configuration.StaticLinksFileName))
        {
            var exampleFileData = new StaticLinksFile();
            exampleFileData.Groups["Group Name"] = new []{
                new StaticLink{
                    Name = "Link Name",
                    Url = "http://linkaddress.com",
                    Roles = new []{"Role Name"}
                }
            };
            var exampleFileJson = JsonConvert.SerializeObject(exampleFileData, Formatting.Indented);
            await File.WriteAllTextAsync(configuration.StaticLinksFileName, exampleFileJson);
        }
        var fileJson = await File.ReadAllTextAsync(configuration.StaticLinksFileName);
        var fileData = JsonConvert.DeserializeObject<StaticLinksFile>(fileJson);
        if (fileData == null)
        {
            throw new InvalidDataException("Static Links File could not be parsed.");
        }
        foreach (var keyValuePair in fileData.Groups)
        {
            var group = keyValuePair.Key;
            foreach (var link in keyValuePair.Value)
            {
                var name = link.Name;
                var url = link.Url;
                yield return new CommonLinkItem(name, group, url, link.Roles);
            }
        }
    }
}

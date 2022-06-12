using Microsoft.Extensions.Configuration;

namespace SwarmPortal.Source;

public class SQLiteSourceConfiguration : ISQLiteSourceConfiguration
{
    public static ISQLiteSourceConfiguration Create(IConfiguration config)
    {
        var sqlSourceConfig = new SQLiteSourceConfiguration();
        config.Bind(sqlSourceConfig);
        return sqlSourceConfig;
    }
    private  SQLiteSourceConfiguration()
    {
    }
    public string SQLiteFileDirectory { get; set; } = "persist";
}
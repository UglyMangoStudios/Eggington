using Microsoft.Extensions.Logging;
using Eggington.Contents.Attributes;

namespace Eggington.Contents.Options;

[RegisterOption("Database")]
internal class DatabaseOptions
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    public DatabaseTypes DatabaseType { get; set; } = DatabaseTypes.Localhost;

    public string ConnectionString { get; set; } = "";

    public string DatabaseName { get; set; } = "";

    public string BuildSQLitePath() =>
        Path.Join(AppDomain.CurrentDomain.BaseDirectory, DatabaseName + ".db");


    public enum DatabaseTypes
    {
        Localhost, Mongo, SQL
    }

}

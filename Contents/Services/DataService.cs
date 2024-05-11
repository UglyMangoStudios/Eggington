using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Eggington.Contents.Attributes;
using Eggington.Contents.Extensions;
using Eggington.Contents.Modals;
using Eggington.Contents.Options;
using Eggington.Contents.Types;
using Serilog;

namespace Eggington.Contents.Services;

[RegisterSingleton(Startup = true)]
internal sealed class DataService : DbContext
{
    public DataService(IOptions<DatabaseOptions> databaseOptions)
    {
        DatabaseOptions = databaseOptions.Value;
    }

    private DatabaseOptions DatabaseOptions { get; }

    public DbSet<UserData> UserData { get; set; }
    public DbSet<GuildData> GuildData { get; set; }

    private ILogger Logger { get; } = Log.ForContext<DataService>();

    protected override void OnConfiguring(DbContextOptionsBuilder opBuilder)
    {
        opBuilder.LogTo((id, level) => level >= DatabaseOptions.LogLevel, data => data.ToLogger(Logger));
        opBuilder.UseLazyLoadingProxies();

        Logger.Information("Configuring database context as {type}", DatabaseOptions.DatabaseType);

        if (DatabaseOptions.DatabaseType == DatabaseOptions.DatabaseTypes.Localhost)
        {
            
            opBuilder.UseSqlite($"Data Source={DatabaseOptions.BuildSQLitePath()}");
        }
        else
        {
            Logger.Fatal("SQLite database is the only database currently supported! No database connect is properly created!");
            // TODO: Add other database logic types
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder config)
    {
        config.Properties<ExpoNumber>().HaveConversion<ExpoNumberValueConverter, ExpoNumberValueComparer>();
    }

    public async Task<UserData> GetUserData(ulong userId)
    {
        var data = await UserData.FirstOrDefaultAsync(data => data.Id == userId);
        if (data is null)
        {
            data = new UserData(userId);
            Add(data);
            await SaveChangesAsync();
        }

        return data;
    }

    public async Task UpdateUserData(UserData userData)
    {
        Entry(userData).State = EntityState.Detached;
        Update(userData);
        await SaveChangesAsync();
    }

    public async Task<GuildData> GetGuildData(ulong guildId)
    {
        var data = await GuildData.FirstOrDefaultAsync(data => data.Id == guildId);
        if (data is null)
        {
            data = new GuildData(guildId);
            Add(data);
            await SaveChangesAsync();
        }

        return data;
    }

    public async Task UpdateGuildData(GuildData guildData)
    {
        Entry(guildData).State = EntityState.Detached;
        Update(guildData);
        await SaveChangesAsync();
    }
}

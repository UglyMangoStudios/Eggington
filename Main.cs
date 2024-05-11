using Microsoft.Extensions.Configuration;
using Eggington.Contents;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;


// Building the application configuration
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<App>(optional: false)
    .Build();

// Building a default logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
    .WriteTo.File("./logs/app.log", rollingInterval: RollingInterval.Month)
    .CreateLogger();

// Builds the application
Log.Information("Building new Eggington (c) Application");
App app = new(configuration);

// Will run forever or until the app closes
await app.Run();

// Closing this up
Log.Information("Eggington (c) Application Session Ending.");
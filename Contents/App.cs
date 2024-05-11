using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eggington.Contents.Extensions;
using Eggington.Contents.Services;
using Eggington.Contents.Types;
using Serilog;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Eggington.Contents;

internal sealed class App
{
    public App(IConfiguration configuration)
    {
        Configuration = configuration;
        ServiceProvider = AddServices();
    }

    private ILogger Logger { get; } = Log.ForContext<App>();

    private IConfiguration Configuration { get; }
    private IServiceProvider ServiceProvider { get; }

    private IReadOnlyList<Type> StartupSingletons { get; set; } = [];
    private IReadOnlyList<Type> Options { get; set; } = [];

    private Dictionary<Type, object> RequiredServices { get; } = [];

    private ServiceProvider AddServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton(this);

        StartupSingletons = services.AutoDiscoverSingletons();
        Options = services.AutoDiscoverOptions(Configuration);

        return services.BuildServiceProvider();
    }

    public async Task Run()
    {
        MultiTask multi = new();
        foreach (var singletonType in StartupSingletons)
        {
            var service = ServiceProvider.GetRequiredService(singletonType);
            RequiredServices[singletonType] = service;
        }

        var dataService = (DataService)RequiredServices[typeof(DataService)];
        multi <<= dataService.Database.EnsureCreatedAsync();

        var botService = (BotService)RequiredServices[typeof(BotService)];
        multi <<= botService.Start();

        // Must be the final line. This line will pass if/when the bot closes.
        await multi.WaitAllAsync();
    }
}

using Discord;
using Discord.Interactions;
using Eggington.Contents.Attributes;
using Eggington.Contents.Types;
using Serilog;
using System.Reflection;

namespace Eggington.Contents.Extensions;

internal static class InteractionExtensions
{
    public static void DiscoverModules(this InteractionService interactionService, IServiceProvider serviceProvider, IEnumerable<IGuild> guilds, ILogger logger)
    {
        logger.Information("Beginning module discover to assign to the interaction service.");

        int count = 0;

        List<ModuleInfo> globalModules = [];
        List<ModuleInfo> guildModules = [];

        MultiTask multi = new ();
        foreach (var type in typeof(App).Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || !type.IsClass || !type.IsAssignableTo(typeof(InteractionModuleBase)))
                continue;

            var attribute = type.GetCustomAttribute<RegisterModuleAttribute>();
            if (attribute is null) continue;

            multi.Run(async () =>
            {
                var moduleInfo = await interactionService.AddModuleAsync(type, serviceProvider);
                if (attribute.IsGlobal)
                    globalModules.Add(moduleInfo);
                else
                    guildModules.Add(moduleInfo);
            });

            count++;
        }
        multi.WaitAll();

        multi <<= interactionService.AddModulesGloballyAsync(true, globalModules.ToArray());
        foreach(var guild in guilds)
        {
            interactionService.AddModulesToGuildAsync(guild, true, guildModules.ToArray());
        }

        multi.WaitAll();

        if (count > 0)
            logger.Information("Successfully registered {count} modules", count);
        else
            logger.Warning("No modules could be found. Nothing was registerd.");
    }

    public static void DiscoverModulesForGuild(this InteractionService interactionService, IServiceProvider serviceProvider, IGuild guild, ILogger logger)
    {
        int count = 0;

        List<ModuleInfo> globalModules = [];
        List<ModuleInfo> guildModules = [];

        var getModuleInfoGeneric = interactionService.GetType().GetMethod("GetModuleInfo")!;

        MultiTask multi = new();
        foreach (var type in typeof(App).Assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface || !type.IsClass || !type.IsAssignableTo(typeof(InteractionModuleBase)))
                continue;

            var attribute = type.GetCustomAttribute<RegisterModuleAttribute>();
            if (attribute is null) continue;

            multi.Run(async () =>
            {
                var getModuleInfoMethod = getModuleInfoGeneric.MakeGenericMethod(type);

                var moduleInfo = (ModuleInfo)getModuleInfoMethod.Invoke(interactionService, null)!;

                if (attribute.IsGlobal)
                    globalModules.Add(moduleInfo);
                else
                    guildModules.Add(moduleInfo);
            });

            count++;
        }
        multi.WaitAll();

        multi <<= interactionService.AddModulesToGuildAsync(guild, true, guildModules.ToArray());

        if (count > 0)
            logger.Information("Successfully registered {count} modules to guild {guild}", count, guild.Name);
        else
            logger.Warning("No modules could be found for guild {guild}. Nothing was registerd.", guild.Name);
    }
}

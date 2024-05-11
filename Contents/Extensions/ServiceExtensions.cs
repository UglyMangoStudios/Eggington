using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eggington.Contents.Attributes;
using System.Reflection;

namespace Eggington.Contents.Extensions;
internal static class ServiceExtensions
{
    public static IReadOnlyList<Type> AutoDiscoverOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        List<Type> options = [];
        var filter = typeof(App).Assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<RegisterOptionAttribute>() is not null)
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<RegisterOptionAttribute>()!
            });

        // Gets the generic configure method so we can dynamically call this extension
        var genericConfigureMethod = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod("Configure", [typeof(IServiceCollection), typeof(IConfiguration)])!;

        foreach (var optionType in filter)
        {
            var type = optionType.Type;
            var section = optionType.Attribute.Section;

            var configure = genericConfigureMethod.MakeGenericMethod(type);
            configure.Invoke(null, [collection, configuration.GetSection(section)]);

            options.Add(type);
        }

        return options.AsReadOnly();
    }

    /// <summary>
    /// Collects and assigns single services within this assemply. All services must be marked with <see cref="RegisterSingletonAttribute"/> at the class level.
    /// </summary>
    /// <param name="collection">The service collection provider to add the singleton to</param>
    /// <returns>A readonly list composed of types flagged with the startup value being true.</returns>
    public static IReadOnlyList<Type> AutoDiscoverSingletons(this IServiceCollection collection)
    {
        List<Type> options = [];
        var filter = typeof(App).Assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<RegisterSingletonAttribute>() is not null)
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<RegisterSingletonAttribute>()!
            });

        foreach (var serviceType in filter)
        {
            var type = serviceType.Type;
            var registration = serviceType.Attribute;

            collection.AddSingleton(type);

            if (registration.Startup)
                options.Add(type);
        }

        return options.AsReadOnly();
    }
}

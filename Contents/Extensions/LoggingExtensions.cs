using Discord;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace Eggington.Contents.Extensions
{
    internal static class LoggingExtensions
    {
        public static Task ToLogger(this LogMessage message, ILogger? logger = null)
        {
            logger ??= Log.Logger;

            Action<string> action = message.Severity switch
            {
                LogSeverity.Critical => logger.Fatal,
                LogSeverity.Error => logger.Error,
                LogSeverity.Warning => logger.Warning,
                LogSeverity.Info => logger.Information,
                LogSeverity.Verbose => logger.Verbose,
                LogSeverity.Debug => logger.Debug,
                _ => logger.Debug,
            };

            action.Invoke(message.Message);

            return Task.CompletedTask;
        }

        public static void ToLogger(this EventData data, ILogger? logger = null)
        {
            logger ??= Log.Logger;

            Action<string> action = data.LogLevel switch
            {
                Microsoft.Extensions.Logging.LogLevel.Trace => logger.Verbose,
                Microsoft.Extensions.Logging.LogLevel.Debug => logger.Debug,
                Microsoft.Extensions.Logging.LogLevel.Information => logger.Information,
                Microsoft.Extensions.Logging.LogLevel.Warning => logger.Warning,
                Microsoft.Extensions.Logging.LogLevel.Error => logger.Error,
                Microsoft.Extensions.Logging.LogLevel.Critical => logger.Fatal,
                Microsoft.Extensions.Logging.LogLevel.None => logger.Debug,
                _ => logger.Debug
            };

            action.Invoke(data.ToString());
        }

    }
}

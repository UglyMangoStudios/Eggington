using Discord.Interactions;
using Discord.WebSocket;
using Eggington.Contents.Attributes;

namespace Eggington.Contents.Options
{

    [RegisterOption("Bot")]
    internal sealed class BotOptions
    {
        public string Token { get; set; } = "";
        public int Permission { get; set; } = 0;

        public DiscordSocketConfig CreateBotConfig()
        {
            DiscordSocketConfig config = new();

            // Add config entries from the bot options here

            return config;
        }

        public InteractionServiceConfig CreateInteractionConfig()
        {
            InteractionServiceConfig config = new();

            // Add config entries from the bot options here

            return config;
        }
    }
}

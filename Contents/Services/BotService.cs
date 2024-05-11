using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Eggington.Contents.Attributes;
using Eggington.Contents.Extensions;
using Eggington.Contents.Options;
using Eggington.Contents.Utility;
using Serilog;

namespace Eggington.Contents.Services;

[RegisterSingleton(Startup = true)]
internal sealed class BotService
{
    public const string INVITE_URL = "https://discord.com/oauth2/authorize?client_id={0}&permissions={1}&scope=applications.commands+bot";

    public BotService(IOptions<BotOptions> botOptions, IServiceProvider serviceProvider)
    {
        BotOptions = botOptions.Value;
        ServiceProvider = serviceProvider;

        Client = CreateClient();
        InteractionService = CreateInteractionService(Client);
    }

    public DiscordSocketClient Client { get; }

    public InteractionService InteractionService { get; }

    private ILogger Logger { get; } = Log.ForContext<BotService>();

    private IServiceProvider ServiceProvider { get; }

    private BotOptions BotOptions { get; }

    /// <summary>
    /// Begins the bot
    /// </summary>
    /// <returns>An awaitable task</returns>
    public async Task Start()
    {
        Logger.Information("Starting Discord Bot Service");

        await Client.LoginAsync(TokenType.Bot, BotOptions.Token);
        await Client.StartAsync();

        // Waits indefinitely
        await Task.Delay(-1);
    }

    private DiscordSocketClient CreateClient()
    {
        var config = BotOptions.CreateBotConfig();
        var client = new DiscordSocketClient(config);

        // Subscribe all events here
        client.Log += msg => msg.ToLogger(Logger);
        client.Ready += ClientReady;
        client.JoinedGuild += OnJoinedGuild;
        client.InteractionCreated += InteractionCreated;

        return client;
    }

    private InteractionService CreateInteractionService(DiscordSocketClient client)
    {
        var config = BotOptions.CreateInteractionConfig();
        InteractionService interaction = new(client, config);

        interaction.InteractionExecuted += InteractionExecuted;

        return interaction;
    }

    private Task ClientReady()
    {
        // Fill client ready logic here
        InteractionService.DiscoverModules(ServiceProvider, Client.Guilds, Logger);

        var user = Client.CurrentUser;

        Logger.Information("Your bot is ready! Invite your bot here: {url}", string.Format(INVITE_URL, user.Id, BotOptions.Permission));

        return Task.CompletedTask;
    }

    private async Task InteractionCreated(SocketInteraction interaction)
    {
        Logger.Information("Interaction created for user {user}", interaction.User.GlobalName);
        var ctx = new SocketInteractionContext(Client, interaction);
        await InteractionService.ExecuteCommandAsync(ctx, ServiceProvider);
    }

    private async Task InteractionExecuted(ICommandInfo info, IInteractionContext context, IResult result)
    {
        if (!result.IsSuccess)
        {
            EmbedBuilder embed = result.Error switch
            {
                InteractionCommandError.UnknownCommand => EmbedUtil.Warn("You used an unknown", result.ErrorReason),

                InteractionCommandError.ConvertFailed => EmbedUtil.Warn("Conversion failed", result.ErrorReason),

                InteractionCommandError.BadArgs => EmbedUtil.Warn("You provided invalid arguments", result.ErrorReason),

                InteractionCommandError.Exception when result is ExecuteResult execute => execute.Exception.InnerException?.ToEmbedBuilder() ?? execute.Exception.ToEmbedBuilder(),
                InteractionCommandError.Exception => EmbedUtil.Warn("An unknown exception occured", result.ErrorReason),

                InteractionCommandError.Unsuccessful => EmbedUtil.Warn("Your interaction was unsuccessful", result.ErrorReason),

                InteractionCommandError.UnmetPrecondition => EmbedUtil.Warn("Preconditions were unmet", result.ErrorReason),

                InteractionCommandError.ParseFailed => EmbedUtil.Warn("Parsing failed", result.ErrorReason),

                _ => EmbedUtil.Warn("An unknown error occured", result.ErrorReason),
            };

            await context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }

    private async Task OnJoinedGuild(SocketGuild guild)
    {
        Logger.Information("Bot was added to guild {guild}. Running necessary steps.", guild.Name);
        InteractionService.DiscoverModulesForGuild(ServiceProvider, guild, Logger);
        await Task.CompletedTask;
    }



}

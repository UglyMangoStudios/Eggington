using Discord;
using Discord.Interactions;

namespace Eggington.Contents.Modules;

internal class BaseModule : InteractionModuleBase
{
    protected IUser User => this.Context.User;
    protected ulong UserId => this.Context.User.Id;
    protected ulong? GuildId => this.Context.Guild?.Id;

    protected async Task RespondEmbedAsync(Embed embed, bool ephemeral = false) =>
        await RespondAsync(embed: embed, ephemeral: ephemeral);

    protected async Task RespondEmbedAsync(EmbedBuilder builder, bool ephemeral = false) =>
        await RespondEmbedAsync(builder.Build(), ephemeral);
}

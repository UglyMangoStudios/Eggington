using Discord;
using Discord.Interactions;
using Eggington.Contents.Attributes;
using Eggington.Contents.Modals;
using Eggington.Contents.Services;
using System.Text;

namespace Eggington.Contents.Modules;

[RegisterModule]
internal class MeModule(DataService dataService) : BaseModule
{
    public DataService DataService { get; } = dataService;

    [SlashCommand("me", "Gives more information about yourself")]
    public async Task MeCommand(IUser? target = null)
    {
        ulong id = target?.Id ?? UserId;
        UserData user = await DataService.GetUserData(id);

        EmbedBuilder embed = new EmbedBuilder().WithTitle("You are " + User.GlobalName);

        var b = new StringBuilder()
            .AppendLine($"🪙 Goat Coins: {user.GoatCoins}")    
        ;

        embed.WithDescription(b.ToString());

        await RespondEmbedAsync(embed);
    }

}

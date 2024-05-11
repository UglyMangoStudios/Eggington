using Discord;
using Eggington.Contents.Utility;

namespace Eggington.Contents.Extensions;

internal static class ExceptionExtensions
{
    public static EmbedBuilder ToEmbedBuilder(this Exception exception, string title = "An Exception Occured") =>
        EmbedUtil.Alert(title, exception.Message);

    public static Embed ToEmbed(this Exception exception, string title = "An Exception Occured") 
        => exception.ToEmbedBuilder(title).Build();

}

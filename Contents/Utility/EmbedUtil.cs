using Discord;

namespace Eggington.Contents.Utility;

internal static class EmbedUtil
{
    //Color Palette: https://flatuicolors.com/palette/defo

    /// <summary> A simple embed. Not much to offer, but at least its an embed. </summary>
    public static EmbedBuilder Basic(string title, params string[] description) =>
        CreateEmbedBuilder(title, stitchStringArray(description), (149, 165, 166));

    /// <summary> An embed used for successful operations </summary>
    public static EmbedBuilder Success(string title, params string[] description) =>
        CreateEmbedBuilder(":white_check_mark:   Success. " + title, stitchStringArray(description), (46, 204, 113));

    /// <summary> An embed used for failed operations </summary>
    public static EmbedBuilder Failure(string title, params string[] description) =>
        CreateEmbedBuilder(":x:   Failed. " + title, stitchStringArray(description), (231, 76, 60));

    /// <summary> An embed used as a general notification alert </summary>
    public static EmbedBuilder Notify(string title, params string[] description) =>
        CreateEmbedBuilder(":bell:   Ding ding! " + title, stitchStringArray(description), (230, 126, 34));

    /// <summary> An embed that mildly offers a suggestion. </summary>
    public static EmbedBuilder Proclaim(string title, params string[] description) =>
        CreateEmbedBuilder(":smile:   Hey! " + title, stitchStringArray(description), (26, 188, 156));

    /// <summary> An embed thats timid yet corrective, saying something didn't work. Usually from a user's error. </summary>
    public static EmbedBuilder Apologize(string title, params string[] description) =>
        CreateEmbedBuilder(":sweat_smile:   Oops, sorry! " + title, stitchStringArray(description), (243, 156, 18));

    /// <summary> An embed that serves a cautionary message. </summary>
    public static EmbedBuilder Warn(string title, params string[] description) =>
        CreateEmbedBuilder(":warning:   Warning! " + title, stitchStringArray(description), (241, 196, 15));

    /// <summary> An attention-grabbing embed that alerts users of something serious. </summary>
    public static EmbedBuilder Alert(string title, params string[] description) =>
        CreateEmbedBuilder(":rotating_light:   Attention! " + title, stitchStringArray(description), (231, 76, 60));

    /// <summary> An over-dramatic screaming embed for something extremely critical </summary>
    public static EmbedBuilder Scream(string title, params string[] description) =>
        CreateEmbedBuilder(":scream:   AHHHH! " + title, stitchStringArray(description), (192, 57, 43));

    /// <summary> For debugging purposes. Used as a way to convey an unexpected error occured. </summary>
    /// <remarks>Returns an already built embed, so no new information can be added.</remarks>
    public static Embed Error(string error)
    {
        Console.WriteLine("A guild received an awful error:\n" + error);
        return Scream("We encountered a bad error!", "Let server staff know of this error ASAP!!", "Error: " + error).Build();
    }

    public static EmbedBuilder Construction(params string[] description) =>
        Apologize("Under Construction!", description);

    public static Embed Loading() => CreateEmbedBuilder(":alarm_clock:    Loading...", "", (241, 196, 15)).Build();


    /// <summary> Strings an array of strings into one string. </summary>
    private static string stitchStringArray(params string[] strings) => string.Join("\n", strings);

    /// <summary>
    /// Handy method that creates an embed instead of the verbose and vanilla method through Discord.NET
    /// </summary>
    /// <param name="title">The title of the embed.</param>
    /// <param name="description">The description of the embed</param>
    /// <param name="color">The color of the embed. It's displayed as a strip on the left side. Each of the RGB components is expressed 0-255</param>
    /// <param name="withTimestamp">A boolean to toggle if the current time should be displayed or not.</param>
    /// <returns>An <see cref="EmbedBuilder"/> that can continue being modified.</returns>
    public static EmbedBuilder CreateEmbedBuilder(string title, string description, (byte r, byte g, byte b) color, bool withTimestamp = true)
    {
        EmbedBuilder builder = new();

        builder
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(color.r, color.g, color.b);

        if (withTimestamp) builder.WithCurrentTimestamp();

        return builder;
    }
}

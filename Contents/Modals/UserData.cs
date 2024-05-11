using Discord;
using Eggington.Contents.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eggington.Contents.Modals;

internal class UserData(ulong id)
{
    [Key]
    public ulong Id { get; private set; } = id;

    public ExpoNumber GoatCoins { get; set; } = 0;
}

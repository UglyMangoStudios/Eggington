using Eggington.Contents.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eggington.Contents.Modals;

internal class GuildData(ulong id)
{
    [Key]
    public ulong Id { get; private set; } = id;
}

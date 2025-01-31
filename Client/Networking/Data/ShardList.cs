namespace Client.Networking.Data;
using Client.Game.Data;
public sealed class ShardList
{
    public static ShardList View { get; } = new();
    public ShardEntry[]? Shards { get; set; }
    public CharInfo[]? Characters { get; set; }
}
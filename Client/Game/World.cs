namespace Client.Game;
using Client.Game.Context;
using Client.Game.Data;

public sealed class World : WorldContext
{
    public const uint ItemOffset = 0x40000000;
    public const uint MaxItemSerial = 0x7EEEEEEE;
    public World() : base((Serial)0) { }
    public static void Configure()
    {
        // TODO
    }
}
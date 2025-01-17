namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public class Map
{
    [PacketHandler(0x8, length: 6, ingame: true, extCmd: true)]
    public static event PacketEventHandler<MapChangeEventArgs>? OnChange;

    [PacketHandler(0x56, length: 11, ingame: true)]
    public static event PacketEventHandler<MapCommandEventArgs>? OnCommand;

    //[Obsolete] public static event PacketEventHandler<MapDetailsEventArgs>? OnDetails;

    [PacketHandler(0x18, length: 33, ingame: true, extCmd: true)]
    public static event PacketEventHandler<MapPatchesEventArgs>? OnPatch;
}
namespace Client.Networking.Arguments;
using Client.Game;
public sealed class MapChangeEventArgs : EventArgs
{
    [PacketHandler(0x8, length: 6, ingame: true, extCmd: true)]
    private static event PacketEventHandler<MapChangeEventArgs>? Update;
    public NetState State { get; }
    public byte Index { get; set; }
    private MapChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Index = ip.ReadByte();
    }
    static MapChangeEventArgs() => Update += MapChangeEventArgs_Update;
    private static void MapChangeEventArgs_Update(MapChangeEventArgs e)
        => Map.InvokeChange(e.State, e.Index);
}
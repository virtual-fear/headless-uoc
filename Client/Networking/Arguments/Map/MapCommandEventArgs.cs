namespace Client.Networking.Arguments;
using Client.Game;
public sealed class MapCommandEventArgs : EventArgs
{

    [PacketHandler(0x56, length: 11, ingame: true)]
    private static event PacketEventHandler<MapCommandEventArgs>? Update;
    public NetState State { get; }
    public int MapItem { get; set; }
    public byte Command { get; set; }
    public byte Number { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    private MapCommandEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        MapItem = ip.ReadInt32();
        Command = ip.ReadByte();
        Number = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
    }

    static MapCommandEventArgs() => Update += MapCommandEventArgs_Update;
    private static void MapCommandEventArgs_Update(MapCommandEventArgs e)
        => Map.Command(e.State, e.MapItem, e.Command, e.Number, e.X, e.Y);
}
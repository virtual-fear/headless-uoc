namespace Client.Networking.Incoming;
public sealed class MapCommandEventArgs : EventArgs
    {
        public NetState State { get; }
        public MapCommandEventArgs(NetState state) => State = state;
        public int MapItem { get; set; }
        public byte Command { get; set; }
        public byte Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
public partial class Map
{
    public static event PacketEventHandler<MapCommandEventArgs>? OnCommand;

    [PacketHandler(0x56, length: 11, ingame: true)]
    protected static void ReceivedMap_Command(NetState ns, PacketReader pvSrc)
    {
        MapCommandEventArgs e = new MapCommandEventArgs(ns);
        e.MapItem = pvSrc.ReadInt32();
        e.Command = pvSrc.ReadByte();
        e.Number = pvSrc.ReadByte();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        OnCommand?.Invoke(e);
    }
}
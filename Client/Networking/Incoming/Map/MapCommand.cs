
namespace Client.Networking.Incoming.Map;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MapCommandEventArgs>? MapUpdateCommand;
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
    protected static class MapCommand
    {
        [PacketHandler(0x56, length: 11, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MapCommandEventArgs e = new MapCommandEventArgs(ns);

            e.MapItem = pvSrc.ReadInt32();
            e.Command = pvSrc.ReadByte();
            e.Number = pvSrc.ReadByte();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            MapUpdateCommand?.Invoke(e);
        }
    }
}

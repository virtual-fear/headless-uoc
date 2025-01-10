namespace Client.Networking.Incoming;
{
[Obsolete("unused")]
public sealed class MapDetailsEventArgs : EventArgs
    {
        public NetState State { get; }
        public MapDetailsEventArgs(NetState state) => State = state;
        public int MapItem { get; set; }
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XEnd { get; set; }
        public int YEnd { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
public partial class Map
{
    [Obsolete("unused")]
    public static event PacketEventHandler<MapDetailsEventArgs>? OnDetails;

    [Obsolete]
    private static void ReceivedMap_Details(NetState ns, PacketReader pvSrc)
    {
        MapDetailsEventArgs e = new MapDetailsEventArgs(ns);
        e.MapItem = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x193D
        e.XStart = pvSrc.ReadInt16();
        e.YStart = pvSrc.ReadInt16();
        e.XEnd = pvSrc.ReadInt16();
        e.YEnd = pvSrc.ReadInt16();
        e.Width = pvSrc.ReadInt16();
        e.Height = pvSrc.ReadInt16();
        OnDetails?.Invoke(e);
    }
}
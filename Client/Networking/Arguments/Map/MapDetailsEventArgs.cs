namespace Client.Networking.Arguments;
public sealed class MapDetailsEventArgs : EventArgs
{
    // TODO: Add [PacketHandler]
    private static event PacketEventHandler<MapDetailsEventArgs>? Update;
    public NetState State { get; }
    public int MapItem { get; set; }
    public int XStart { get; set; }
    public int YStart { get; set; }
    public int XEnd { get; set; }
    public int YEnd { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    [Obsolete("Unused", error: true)]
    private MapDetailsEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        MapItem = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x193D
        XStart = pvSrc.ReadInt16();
        YStart = pvSrc.ReadInt16();
        XEnd = pvSrc.ReadInt16();
        YEnd = pvSrc.ReadInt16();
        Width = pvSrc.ReadInt16();
        Height = pvSrc.ReadInt16();
    }

    static MapDetailsEventArgs() => Update += MapDetailsEventArgs_Update;
    private static void MapDetailsEventArgs_Update(MapDetailsEventArgs e)
    {
    }
}
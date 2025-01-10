namespace Client.Networking.Incoming;
public sealed class MapChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public PacketReader? Reader { get; set; }
    public byte Index { get; set; }
    public MapChangeEventArgs(NetState state) => State = state;
}
public partial class Map
{
    public static event PacketEventHandler<MapChangeEventArgs>? OnChange;

    [PacketHandler(0x8, length: 6, ingame: true, extCmd: true)]
    protected static void ReceivedMap_Change(NetState ns, PacketReader pvSrc)
    {
        MapChangeEventArgs e = new MapChangeEventArgs(ns)
        {
            Reader = pvSrc,
            Index = pvSrc.ReadByte()
        };
        OnChange?.Invoke(e);
    }
}
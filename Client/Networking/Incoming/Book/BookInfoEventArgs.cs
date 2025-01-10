namespace Client.Networking.Incoming;
public sealed class BookInfoEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; }
    public BookInfoEventArgs(NetState state, bool supported)
    {
        State = state;
        Supported = supported;
    }
}
public partial class Book
{
    public static event PacketEventHandler<BookInfoEventArgs>? Info;

    [PacketHandler(0x66, length: -1, ingame: true)]
    protected static void ReceivedBook_Info(NetState ns, PacketReader pvSrc)
    {
        BookInfoEventArgs e = new(ns, false);
        pvSrc.ReadInt32();
        pvSrc.ReadUInt16();
        Info?.Invoke(e);
    }
}

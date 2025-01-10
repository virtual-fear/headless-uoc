namespace Client.Networking.Incoming;
public sealed class BookOpenEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; }
    public BookOpenEventArgs(NetState state, bool supported)
    {
        State = state;
        Supported = supported;
    }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}
public partial class Book
{
    public static event PacketEventHandler<BookOpenEventArgs>? Open;

    [PacketHandler(0xD4, length: -1, ingame: true)]
    protected static void ReceivedBook_Open(NetState ns, PacketReader pvSrc)
    {
        BookOpenEventArgs e = new(ns, false);
        pvSrc.ReadInt32();
        pvSrc.ReadBoolean();
        pvSrc.ReadBoolean();
        pvSrc.ReadInt16();
        e.Title = pvSrc.ReadString(pvSrc.ReadInt16());
        e.Author = pvSrc.ReadString(pvSrc.ReadInt16());
        Open?.Invoke(e);
    }
}
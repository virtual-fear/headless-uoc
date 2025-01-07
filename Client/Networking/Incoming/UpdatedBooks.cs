namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    #region EventArgs
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
    public sealed class BookOpenEventArgs : EventArgs
    {
        public NetState State { get; }
        public bool Supported { get; }
        public BookOpenEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
        public string Title { get; set; }
        public string Author { get; set; }
    }
    #endregion (done)

    public static event PacketEventHandler<BookOpenEventArgs>? BookOpen;
    public static event PacketEventHandler<BookInfoEventArgs>? BookInfo;
    public static void InvokeBookInfo(BookInfoEventArgs e) => BookInfo?.Invoke(e);
    public static void InvokeBookOpen(BookOpenEventArgs e) => BookOpen?.Invoke(e);
}

public static class UpdatedBooks
{
    public static void Configure()
    {
        Register(0xD4, -1, true, new OnPacketReceive(BookOpen));
        Register(0x66, -1, true, new OnPacketReceive(BookInfo));
    }
    private static void BookInfo(NetState ns, PacketReader pvSrc)
    {
        BookInfoEventArgs e = new BookInfoEventArgs(ns, false);

        pvSrc.ReadInt32();
        pvSrc.ReadInt16();  //  actually: unsigned

        PacketSink.InvokeBookInfo(e);
    }
    private static void BookOpen(NetState ns, PacketReader pvSrc)
    {
        BookOpenEventArgs e = new BookOpenEventArgs(ns, false);

        pvSrc.Seek(8, SeekOrigin.Current);
        // Int32(1), Bool(2), Int16(1) //

        e.Title = pvSrc.ReadString(pvSrc.ReadInt16());
        e.Author = pvSrc.ReadString(pvSrc.ReadInt16());

        PacketSink.InvokeBookOpen(e);
    }
    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
}

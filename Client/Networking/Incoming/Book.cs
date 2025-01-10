namespace Client.Networking.Incoming;
public partial class PacketHandlers
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
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
    #endregion

    public static event PacketEventHandler<BookInfoEventArgs>? BookInfo;
    public static event PacketEventHandler<BookOpenEventArgs>? BookOpen;
    protected static class Book
    {
        [PacketHandler(0x66, length: -1, ingame: true)]
        public static void Info(NetState ns, PacketReader pvSrc)
        {
            BookInfoEventArgs e = new(ns, false);
            pvSrc.ReadInt32();
            pvSrc.ReadUInt16();
            BookInfo?.Invoke(e);
        }

        [PacketHandler(0xD4, length: -1, ingame: true)]
        public static void Open(NetState ns, PacketReader pvSrc)
        {
            BookOpenEventArgs e = new(ns, false);
            pvSrc.ReadInt32();
            pvSrc.ReadBoolean();
            pvSrc.ReadBoolean();
            pvSrc.ReadInt16();
            e.Title = pvSrc.ReadString(pvSrc.ReadInt16());
            e.Author = pvSrc.ReadString(pvSrc.ReadInt16());
            BookOpen?.Invoke(e);
        }
    }
}
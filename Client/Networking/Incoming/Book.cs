namespace Client.Networking.Incoming;
public partial class Book
{
    public static event PacketEventHandler<BookInfoEventArgs>? Info;
    public static event PacketEventHandler<BookOpenEventArgs>? Open;

    [PacketHandler(0xD4, length: -1, ingame: true)]
    protected static void ReceivedBook_Open(NetState ns, PacketReader ip) => Open?.Invoke(new(ns, ip));

    [PacketHandler(0x66, length: -1, ingame: true)]
    protected static void ReceivedBook_Info(NetState ns, PacketReader ip) => Info?.Invoke(new(ns, ip));
}

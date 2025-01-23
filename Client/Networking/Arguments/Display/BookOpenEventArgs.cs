namespace Client.Networking.Arguments;
using Client.Game;
public sealed class BookOpenEventArgs : EventArgs
{
    // TODO: Add more support for this packet
    [PacketHandler(0xD4, length: -1, ingame: true)]
    private static event PacketEventHandler<BookOpenEventArgs> Update;
    public NetState State { get; }
    public bool Supported { get; } = false;
    public string Title { get; }
    public string Author { get; }
    private BookOpenEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt32();
        ip.ReadBoolean();
        ip.ReadBoolean();
        ip.ReadInt16();
        Title = ip.ReadString(ip.ReadInt16());
        Author = ip.ReadString(ip.ReadInt16());
    }
    static BookOpenEventArgs() => Update += BookOpenEventArgs_Update;
    private static void BookOpenEventArgs_Update(BookOpenEventArgs e)
        => Display.ShowBook(e.State, e.Title, e.Author);
}
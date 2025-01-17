namespace Client.Networking.Arguments;
public sealed class BookOpenEventArgs : EventArgs
{
    [PacketHandler(0xD4, length: -1, ingame: true)]
    public static event PacketEventHandler<BookOpenEventArgs>? Update;

    public NetState State { get; }
    public bool Supported { get; } = false;
    public string Title { get; }
    public string Author { get; }
    internal BookOpenEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt32();
        ip.ReadBoolean();
        ip.ReadBoolean();
        ip.ReadInt16();
        Title = ip.ReadString(ip.ReadInt16());
        Author = ip.ReadString(ip.ReadInt16());
    }
}
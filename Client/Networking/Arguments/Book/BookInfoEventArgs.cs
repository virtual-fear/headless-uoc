namespace Client.Networking.Arguments;
public sealed class BookInfoEventArgs : EventArgs
{
    [PacketHandler(0x66, length: -1, ingame: true)]
    public static event PacketEventHandler<BookInfoEventArgs>? Update;
    public NetState State { get; }
    public bool Supported { get; } = false;
    internal BookInfoEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt32();
        ip.ReadUInt16();
    }
}
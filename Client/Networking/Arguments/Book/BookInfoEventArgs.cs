namespace Client.Networking.Arguments;
public sealed class BookInfoEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; } = false;
    internal BookInfoEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt32();
        ip.ReadUInt16();
    }
}
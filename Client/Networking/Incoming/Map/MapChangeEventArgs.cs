namespace Client.Networking.Incoming;
public sealed class MapChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Index { get; set; }
    internal MapChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Index = ip.ReadByte();
    }
}
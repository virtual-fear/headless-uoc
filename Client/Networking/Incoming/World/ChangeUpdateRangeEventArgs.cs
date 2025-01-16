namespace Client.Networking.Incoming;
public sealed class ChangeUpdateRangeEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Range { get; }
    internal ChangeUpdateRangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Range = ip.ReadByte();
    }
}
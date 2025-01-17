namespace Client.Networking.Arguments;
public sealed class SequenceEventArgs : EventArgs
{
    public NetState State { get; }
    public int Value { get; }
    internal SequenceEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
}
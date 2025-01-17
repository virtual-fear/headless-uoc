namespace Client.Networking.Arguments;
public sealed class GQCountEventArgs : EventArgs
{
    public NetState State { get; }
    public short Unk { get; }
    public int Count { get; }
    internal GQCountEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Unk = ip.ReadInt16();
        Count = ip.ReadInt32();
    }
}
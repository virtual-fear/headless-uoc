namespace Client.Networking.Incoming;
public sealed class NullFastwalkStackEventArgs : EventArgs
{
    public NetState State { get; }
    public byte[]? Buffer { get; }
    internal NullFastwalkStackEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Buffer = ip.ReadBytes(6 * sizeof(int));
    }
}
namespace Client.Networking.Arguments;
public sealed class ServerAckEventArgs : EventArgs
{
    public NetState State { get; }
    public uint Addr { get; }
    public short Port { get; }
    public uint Seed { get; }
    internal ServerAckEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Addr = ip.ReadUInt32LE();
        Port = ip.ReadInt16();
        Seed = ip.ReadUInt32();
    }
}
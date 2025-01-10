namespace Client.Networking.Incoming;
public sealed class ServerAckEventArgs : EventArgs
{
    public uint Addr { get; }
    public short Port { get; }
    public uint Seed { get; }
    internal ServerAckEventArgs(uint addr, short port, uint seed)
    {
        Addr = addr;
        Port = port;
        Seed = seed;
    }
}
public partial class Shard
{
    public static event PacketEventHandler<ServerAckEventArgs>? OnServerAck;

    [PacketHandler(0x8C, length: 11, ingame: false)]
    protected static void Received_ServerAck(NetState state, PacketReader pvSrc)
    {
        uint rawAddress = pvSrc.ReadUInt32LE();
        short port = pvSrc.ReadInt16();
        uint seed = pvSrc.ReadUInt32();
        ServerAckEventArgs e = new(rawAddress, port, seed);
        OnServerAck?.Invoke(e);
    }
}
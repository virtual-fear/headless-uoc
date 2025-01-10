namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ServerAckEventArgs>? Shard_ServerAck;
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
    protected static class ServerAck
    {
        [PacketHandler(0x8C, length: 11, ingame: false)]
        internal static void Receive(NetState state, PacketReader pvSrc)
        {
            uint rawAddress = pvSrc.ReadUInt32LE();
            short port = pvSrc.ReadInt16();
            uint seed = pvSrc.ReadUInt32();
            ServerAckEventArgs e = new(rawAddress, port, seed);
            Shard_ServerAck?.Invoke(e);
        }
    }
}

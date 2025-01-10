namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PingReqEventArgs>? Player_PingAck;
    public sealed class PingReqEventArgs : EventArgs
    {
        public NetState State { get; }
        public PingReqEventArgs(NetState state) => State = state;
        public byte Ping { get; set; }
    }
    
    /// <summary>
    ///     The server is requesting a ping.
    /// </summary>
    protected static class PingAck
    {
        [PacketHandler(0x73, length: 2, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            PingReqEventArgs e = new(ns);
            Packet p = Outgoing.PPing.Instantiate(pvSrc);
            pvSrc.Seek(-1, SeekOrigin.Current);
            e.Ping = pvSrc.ReadByte();
            ns.Send(p);
            Player_PingAck?.Invoke(e);
        }
    }
}
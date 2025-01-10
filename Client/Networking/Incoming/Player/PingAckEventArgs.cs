namespace Client.Networking.Incoming;
public sealed class PingReqEventArgs : EventArgs
{
    public NetState State { get; }
    public PingReqEventArgs(NetState state) => State = state;
    public byte Ping { get; set; }
}
    
/// <summary>
///     The server is requesting a ping.
/// </summary>
public partial class Player
{
    public static event PacketEventHandler<PingReqEventArgs>? OnPingAck;

    [PacketHandler(0x73, length: 2, ingame: true)]
    protected static void Receive_PingAck(NetState ns, PacketReader pvSrc)
    {
        PingReqEventArgs e = new(ns);
        Packet p = Outgoing.PPing.Instantiate(pvSrc);
        pvSrc.Seek(-1, SeekOrigin.Current);
        e.Ping = pvSrc.ReadByte();
        ns.Send(p);
        OnPingAck?.Invoke(e);
    }
}

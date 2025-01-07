namespace Client.Networking.Incoming;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs
    
    public class TargetReqEventArgs : EventArgs
    {
        public NetState State { get; }
        public TargetReqEventArgs(NetState state) => State = state;
        public bool AllowGround { get; set; }
        public int TargetID { get; set; }
        public TargetFlags Flags { get; set; }
    }
    public class PingReqEventArgs : EventArgs
    {
        public NetState State { get; }
        public PingReqEventArgs(NetState state) => State = state;
        public byte Ping { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<TargetReqEventArgs>? TargetReq;
    public static event PacketEventHandler<PingReqEventArgs>? PingReq;
    public static void InvokePingReq(PingReqEventArgs e) => PingReq?.Invoke(e);
    public static void InvokeTargetReq(TargetReqEventArgs e) => TargetReq?.Invoke(e);
}

public enum TargetFlags : byte
{
    None = 0x00,
    Harmful = 0x01,
    Beneficial = 0x02,
}
public static class UpdatedRequests
{
    public static void Configure()
    {
        Register(0x73, 02, true, new OnPacketReceive(PingReq));
        Register(0x6C, 19, true, new OnPacketReceive(TargetReq));
    }

    private static void TargetReq(NetState ns, PacketReader pvSrc)
    {
        TargetReqEventArgs e = new TargetReqEventArgs(ns);

        e.AllowGround = pvSrc.ReadBoolean();
        e.TargetID = pvSrc.ReadInt32();
        e.Flags = (TargetFlags)pvSrc.ReadByte();

        pvSrc.ReadBytes(13);

        PacketSink.InvokeTargetReq(e);
    }

    private static void PingReq(NetState ns, PacketReader pvSrc)
    {
        PingReqEventArgs e = new PingReqEventArgs(ns);

        Packet p = Outgoing.PPing.Instantiate(pvSrc);
        pvSrc.Seek(-1, SeekOrigin.Current);

        e.Ping = pvSrc.ReadByte();

        ns.Send(p);

        PacketSink.InvokePingReq(e);
    }

    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.Register(packetID, length, variable, onReceive);
    }
}

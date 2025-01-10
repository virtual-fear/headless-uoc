namespace Client.Networking.Incoming;
public sealed class ClosedGumpEventArgs : EventArgs
    {
        public NetState State { get; }
        public ClosedGumpEventArgs(NetState state) => State = state;
        public int TypeID { get; set; }
        public int ButtonID { get; set; }
    }
public partial class Gump
{
    public static event PacketEventHandler<ClosedGumpEventArgs>? OnClose;

    [PacketHandler(0x04, length: 13, ingame: true, extCmd: true)]
    protected static void ReceivedGump_Close(NetState ns, PacketReader pvSrc)
    {
        ClosedGumpEventArgs e = new(ns);
        e.TypeID = pvSrc.ReadInt32();
        e.ButtonID = pvSrc.ReadInt32();
        OnClose?.Invoke(e);
    }
}

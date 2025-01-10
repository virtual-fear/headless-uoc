namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileStamEventArgs>? OnMobileStam;
    public sealed class MobileStamEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileStamEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short StamMax { get; set; }
        public short Stam { get; set; }
    }
    protected static class MobileStam
    {
        [PacketHandler(0xA3, length: 9, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileStamEventArgs e = new MobileStamEventArgs(ns);
            e.Serial = pvSrc.ReadInt16();
            e.StamMax = pvSrc.ReadInt16();
            e.Stam = pvSrc.ReadInt16();
            OnMobileStam?.Invoke(e);
        }
    }
}

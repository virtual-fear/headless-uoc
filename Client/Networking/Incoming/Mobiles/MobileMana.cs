namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileManaEventArgs>? OnMobileMana;
    public sealed class MobileManaEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileManaEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ManaMax { get; set; }
        public short Mana { get; set; }
    }
    protected static class MobileMana
    {
        [PacketHandler(0xA3, length: 9, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileManaEventArgs e = new MobileManaEventArgs(ns);
            e.Serial = pvSrc.ReadInt32();
            e.ManaMax = pvSrc.ReadInt16();
            e.Mana = pvSrc.ReadInt16();
            OnMobileMana?.Invoke(e);
        }
    }
}
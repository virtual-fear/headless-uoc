namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileHitsEventArgs>? OnMobileHits;
    public sealed class MobileHitsEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileHitsEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short HitsMax { get; set; }
        public short Hits { get; set; }
    }

    public partial class MobileHits
    {
        [PacketHandler(0xA3, length: 9, ingame: true)]
        protected static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileHitsEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.HitsMax = pvSrc.ReadInt16();
            e.Hits = pvSrc.ReadInt16();
            OnMobileHits?.Invoke(e);
        }
    }
}
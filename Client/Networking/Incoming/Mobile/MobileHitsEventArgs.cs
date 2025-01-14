namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileHitsEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileHitsEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short HitsMax { get; set; }
    public short Hits { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileHitsEventArgs>? OnChangedHits;

    [PacketHandler(0xA1, length: 9, ingame: true)]
    protected static void Received_MobileHits(NetState ns, PacketReader pvSrc)
    {
        MobileHitsEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.HitsMax = pvSrc.ReadInt16();
        e.Hits = pvSrc.ReadInt16();
        OnChangedHits?.Invoke(e);
    }
}
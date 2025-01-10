namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileAttributesEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileAttributesEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short MaxHits { get; set; }
    public short Hits { get; set; }
    public short MaxMana { get; set; }
    public short Mana { get; set; }
    public short MaxStam { get; set; }
    public short Stam { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileAttributesEventArgs>? OnChangedAttributes;

    [PacketHandler(0x2D, length: 17, ingame: true)]
    protected static void Received_Attributes(NetState ns, PacketReader pvSrc)
    {
        MobileAttributesEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.MaxHits = pvSrc.ReadInt16();
        e.Hits = pvSrc.ReadInt16();
        e.MaxMana = pvSrc.ReadInt16();
        e.Mana = pvSrc.ReadInt16();
        e.MaxStam = pvSrc.ReadInt16();
        e.Stam = pvSrc.ReadInt16();
        OnChangedAttributes?.Invoke(e);
    }
}
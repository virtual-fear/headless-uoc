namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileAttributesEventArgs : EventArgs
{
    [PacketHandler(0x2D, length: 17, ingame: true)]
    private static event PacketEventHandler<MobileAttributesEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short MaxHits { get; }
    public short Hits { get; }
    public short MaxMana { get; }
    public short Mana { get; }
    public short MaxStam { get; }
    public short Stam { get; }
    private MobileAttributesEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        MaxHits = ip.ReadInt16();
        Hits = ip.ReadInt16();
        MaxMana = ip.ReadInt16();
        Mana = ip.ReadInt16();
        MaxStam = ip.ReadInt16();
        Stam = ip.ReadInt16();
    }
    static MobileAttributesEventArgs() => Update += MobileAttributesEventArgs_Update;
    private static void MobileAttributesEventArgs_Update(MobileAttributesEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateAttributes(e.Hits, e.MaxHits, e.Mana, e.MaxMana, e.Stam, e.MaxStam);
}
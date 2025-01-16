using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class MobileAttributesEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short MaxHits { get; }
    public short Hits { get; }
    public short MaxMana { get; }
    public short Mana { get; }
    public short MaxStam { get; }
    public short Stam { get; }
    internal MobileAttributesEventArgs(NetState state, PacketReader ip)
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
}
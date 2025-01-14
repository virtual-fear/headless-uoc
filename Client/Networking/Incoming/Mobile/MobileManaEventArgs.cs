namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileManaEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileManaEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short ManaMax { get; set; }
    public short Mana { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileManaEventArgs>? OnChangedMana;

    [PacketHandler(0xA2, length: 9, ingame: true)]
    protected static void Received_MobileMana(NetState ns, PacketReader pvSrc)
    {
        MobileManaEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.ManaMax = pvSrc.ReadInt16();
        e.Mana = pvSrc.ReadInt16();
        OnChangedMana?.Invoke(e);
    }
}
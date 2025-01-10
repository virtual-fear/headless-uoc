namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class DamageEventArgs : EventArgs
{
    public NetState State { get; }
    public DamageEventArgs(NetState state) => State = state;
    public Serial Mobile { get; set; }
    public ushort Amount { get; set; }
}
public sealed class MobileDamageEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileDamageEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public ushort Amount { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<DamageEventArgs>? OnDamage;
    public static event PacketEventHandler<MobileDamageEventArgs>? ReceivedDamage;

    [PacketHandler(0x0B, length: 7, ingame: true)]
    protected static void Received_MobileDamage(NetState ns, PacketReader pvSrc)
    {
        MobileDamageEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.Amount = pvSrc.ReadUInt16();
        ReceivedDamage?.Invoke(e);
    }

    [PacketHandler(0x22, length: 11, ingame: true, extCmd: true)]
    protected static void Receive_Damage(NetState ns, PacketReader pvSrc)
    {
        DamageEventArgs e = new(ns);
        pvSrc.ReadByte();
        e.Mobile = (Serial)pvSrc.ReadUInt32();
        e.Amount = pvSrc.ReadByte();
        OnDamage?.Invoke(e);
    }
}
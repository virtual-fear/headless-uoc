namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DamageEventArgs>? OnDamage;
    public sealed class DamageEventArgs : EventArgs
    {
        public NetState State { get; }
        public DamageEventArgs(NetState state) => State = state;
        public int Mobile { get; set; }
        public int Amount { get; set; }
    }

    protected static class Damage
    {
        [PacketHandler(0x22, length: 11, ingame: true, extCmd: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            DamageEventArgs e = new(ns);
            pvSrc.ReadByte();
            e.Mobile = pvSrc.ReadInt32();
            e.Amount = pvSrc.ReadByte();
            OnDamage?.Invoke(e);
        }
    }
}

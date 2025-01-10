using Client.Game.Data;

namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileDamageEventArgs>? OnMobileDamage;
    public sealed class MobileDamageEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileDamageEventArgs(NetState state) => State = state;
        public Serial Serial { get; set; }
        public ushort Amount { get; set; }
    }

    protected static class MobileDamage
    {
        [PacketHandler(0x0B, length: 7, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileDamageEventArgs e = new(ns);
            e.Serial = (Serial)pvSrc.ReadUInt32();
            e.Amount = pvSrc.ReadUInt16();
            OnMobileDamage?.Invoke(e);
        }
    }
}
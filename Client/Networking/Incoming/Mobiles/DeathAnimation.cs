namespace Client.Networking.Incoming.Mobiles;
using Client.Game.Context;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DeathAnimationEventArgs>? OnDeathAnimation;
    public sealed class DeathAnimationEventArgs : EventArgs
    {
        public NetState State { get; }
        public DeathAnimationEventArgs(NetState state) => State = state;
        public MobileContext Mobile { get; set; }
        public ItemContext Corpse { get; set; }
    }
    protected static class DeathAnimation
    {
        [PacketHandler(0xAF, length: 13, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            DeathAnimationEventArgs e = new(ns);
            e.Mobile = MobileContext.Acquire(pvSrc.ReadInt32());
            e.Corpse = ItemContext.Acquire(pvSrc.ReadInt32());
            pvSrc.ReadInt32();
            OnDeathAnimation?.Invoke(e);
        }
    }
}
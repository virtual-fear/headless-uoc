namespace Client.Networking.Incoming;
using Client.Game.Context;
using Client.Game.Data;
public sealed class DeathAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public DeathAnimationEventArgs(NetState state) => State = state;
    public MobileContext? Mobile { get; set; }
    public ItemContext? Corpse { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<DeathAnimationEventArgs>? OnDeathAnimation;

    [PacketHandler(0xAF, length: 13, ingame: true)]
    protected static void Received_DeathAnimation(NetState ns, PacketReader pvSrc)
    {
        DeathAnimationEventArgs e = new(ns);
        e.Mobile = MobileContext.Acquire((Serial)pvSrc.ReadUInt32());
        e.Corpse = ItemContext.Acquire((Serial)pvSrc.ReadUInt32());
        pvSrc.ReadInt32();
        OnDeathAnimation?.Invoke(e);
    }
}
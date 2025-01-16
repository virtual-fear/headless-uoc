using ItemContext = Client.Game.Context.ItemContext;
using MobileContext = Client.Game.Context.MobileContext;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class DeathAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileContext? Mobile { get; }
    public ItemContext? Corpse { get; }
    internal DeathAnimationEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Mobile = MobileContext.Acquire((Serial)ip.ReadUInt32());
        Corpse = ItemContext.Acquire((Serial)ip.ReadUInt32());
        ip.ReadInt32();
    }
}
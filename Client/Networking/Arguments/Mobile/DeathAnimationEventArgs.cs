namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class DeathAnimationEventArgs : EventArgs
{
    [PacketHandler(0xAF, length: 13, ingame: true)]
    private static event PacketEventHandler<DeathAnimationEventArgs>? Update;
    public NetState State { get; }
    public Mobile? Mobile { get; }
    public Item? Corpse { get; }
    private DeathAnimationEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Mobile = Mobile.Acquire((Serial)ip.ReadUInt32());
        Corpse = Item.Acquire((Serial)ip.ReadUInt32());
        ip.ReadInt32();
    }
    static DeathAnimationEventArgs() => Update += DeathAnimationEventArgs_Update;
    private static void DeathAnimationEventArgs_Update(DeathAnimationEventArgs e)
    {
    }
}
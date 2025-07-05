namespace Client.Networking.Arguments;

using Client.Game;
using Client.Game.Data;
public sealed class LiftRejEventArgs : EventArgs
{
    [PacketHandler(0x27, length: 2, ingame: true)]
    private static event PacketEventHandler<LiftRejEventArgs>? Update;
    public NetState State { get; }
    public LRReason Reason { get; }
    private LiftRejEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Reason = (LRReason)ip.ReadByte();
    }
    static LiftRejEventArgs() => Update += LiftRejEventArgs_Update;
    private static void LiftRejEventArgs_Update(LiftRejEventArgs e) => Player.LiftRej(e.State, e.Reason);
}
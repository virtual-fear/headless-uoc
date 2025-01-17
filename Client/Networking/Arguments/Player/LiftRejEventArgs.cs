using LRReason = Client.Game.Data.LRReason;
namespace Client.Networking.Arguments;
public sealed class LiftRejEventArgs : EventArgs
{
    public NetState State { get; }
    public LRReason Reason { get; }
    internal LiftRejEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Reason = (LRReason)ip.ReadByte();
    }
}
namespace Client.Networking.Incoming.Player;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<LiftRejEventArgs>? Player_LiftRej;
    public sealed class LiftRejEventArgs : EventArgs
    {
        public NetState State { get; }
        public LiftRejEventArgs(NetState state) => State = state;
        public LRReason Reason { get; set; }
    }
    protected static class LiftRej
    {
        [PacketHandler(0x27, length: 2, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            LiftRejEventArgs e = new(ns);
            e.Reason = (LRReason)pvSrc.ReadByte();
            Player_LiftRej?.Invoke(e);
        }
    }
}
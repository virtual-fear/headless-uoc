namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<CancelArrowEventArgs>? Player_CancelArrow;
    public sealed class CancelArrowEventArgs : EventArgs
    {
        public NetState State { get; }
        public CancelArrowEventArgs(NetState state) => State = state;
    }
    protected static class CancelArrow
    {
        [PacketHandler(0xBA, length: 6, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            CancelArrowEventArgs e = new(ns);
            pvSrc.Seek(5, SeekOrigin.Begin);
            Player_CancelArrow?.Invoke(e);
        }
    }
}

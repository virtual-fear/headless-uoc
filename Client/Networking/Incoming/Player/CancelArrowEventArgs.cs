namespace Client.Networking.Incoming;
public sealed class CancelArrowEventArgs : EventArgs
{
    public NetState State { get; }
    public CancelArrowEventArgs(NetState state) => State = state;
}
public partial class Player
{
    public static event PacketEventHandler<CancelArrowEventArgs>? OnCancelArrow;

    [PacketHandler(0xBA, length: 6, ingame: true)]
    protected static void Receive_CancelArrow(NetState ns, PacketReader pvSrc)
    {
        CancelArrowEventArgs e = new(ns);
        pvSrc.Seek(5, SeekOrigin.Begin);
        OnCancelArrow?.Invoke(e);
    }
}
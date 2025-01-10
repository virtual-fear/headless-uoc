namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class LiftRejEventArgs : EventArgs
{
    public NetState State { get; }
    public LiftRejEventArgs(NetState state) => State = state;
    public LRReason Reason { get; set; }
}
public partial class LiftRej
{
    public static event PacketEventHandler<LiftRejEventArgs>? OnLiftRej;

    [PacketHandler(0x27, length: 2, ingame: true)]
    protected static void Receive_LiftRej(NetState ns, PacketReader pvSrc)
    {
        LiftRejEventArgs e = new(ns)
        {
            Reason = (LRReason)pvSrc.ReadByte()
        };
        OnLiftRej?.Invoke(e);
    }
}